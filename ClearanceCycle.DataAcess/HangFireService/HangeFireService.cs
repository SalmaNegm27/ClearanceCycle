﻿using ClearanceCycle.Application.Interfaces;
using ClearanceCycle.DataAcess.Implementation;
using ClearanceCycle.Domain.Entities;
using ClearanceCycle.WorkFlow.Models;
using MediatR.NotificationPublishers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ClearanceCycle.DataAcess.HangFireService
{
    public class HangeFireService : IHangFireService
    {
        private readonly ILogger<WriteRepository> _logger;
        private readonly IExternalService _externalService;
        private readonly AuthDbContext _context;
        private readonly IConfiguration _configuration;

        public HangeFireService(ILogger<WriteRepository> logger, IExternalService externalService, AuthDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _externalService = externalService;
            _context = context;
            _configuration = configuration;
        }
        #region Deactivate accounts 
        public async Task<bool> DeactivateAllEmployeeAccounts()
        {
            var requests = await GetAllRequestToDeactivate();
            if (!requests.Any())
                return false;

            try
            {
                var failedEmployees = new List<string>();

                foreach (var request in requests)
                {
                    try
                    {
                        var apiSuccess = await _externalService.DeactivateEmployeeBusinessAccount(request.Employee.HrId);
                        if (!apiSuccess)
                        {
                            failedEmployees.Add(request.Employee.HrId);
                            _logger.LogError($"Failed to deactivate Rms account for employee {request.Employee.HrId}");
                            continue;
                        }

                        request.Employee.Active = false;
                        request.IsResigneeAccountsClosed = true;
                        request.AccountsClosedAt = DateTime.Now;

                        await _externalService.DeactivateEmployeePortalAccounts(request.ResigneeHrId);
                        await _externalService.DeactivateEmployeeAmanCardAPI(request.Employee.NationalId);
                    }
                    catch (Exception ex)
                    {
                        failedEmployees.Add(request.Employee.HrId);
                        _logger.LogError(ex, $"Error processing employee {request.Employee.HrId}");
                    }
                }

                await _context.SaveChangesAsync();

                if (failedEmployees.Any())
                {
                    _logger.LogWarning($"Some employees failed to deactivate: {string.Join(", ", failedEmployees)}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction failed while deactivating employee accounts");
                return false;
            }
        }

        private async Task<List<ClearanceRequest>> GetAllRequestToDeactivate()
        {

            var clearanceRequests = await _context.ClearanceRequests.Include(c => c.Employee)
                .Where(r => r.LastWorkingDayDate.Date <= DateTime.Today.Date &&
                                          !r.IsCanceled && !r.IsFinished && r.Employee.Active && !r.IsResigneeAccountsClosed).ToListAsync();

            return clearanceRequests;

        }
        #endregion

        #region Escalation Point
        public async Task ProcessEscalation()
        {
            var requests = await GetRequestToEscalation();
            {
                var to = new List<Recipient>();
                var body = _configuration["EmailTemplates:EscalationEmailBody"];
                var subject = _configuration["EmailTemplates:Subject"];
                foreach (var request in requests)
                {
                    List<int> pendingGroups = request.StepApprovalGroup.ApprovalGroups
                                                     .Where(ag => !ag.IsApproved)
                                                     .Select(ag => ag.ApprovalGroupId)
                                                      .ToList();
                    var managers = await GetManagerToEscalate(request.Employee.MajourAreaId, request.Employee.CompanyId, pendingGroups);
                    var email = new EmailServiceDto
                    {
                        Email = new Email
                        {
                            HtmlBody = FormatEmailBody(body, request, managers),
                            Subject = subject,
                            To = managers.Select(man => new Recipient
                            {
                                EmailAddress = new EmailAddress { Address = man }
                            }).ToList()
                        }
                    };
                    await _externalService.SendMails(email);
                }
            }
        }
        private string FormatEmailBody(string bodyTemplate, ClearanceRequest request, List<string> managers)
        {
            var body = bodyTemplate.Replace("[TeamName]", request.StepApprovalGroup.Name);
            return body;
        }
        private async Task<List<ClearanceRequest>> GetRequestToEscalation()
        {
            DateTime beforeTwoDays = DateTime.Today.AddDays(-2);

            return await _context.ClearanceRequests
                                .Include(c => c.Employee)
                                  .Include(c => c.StepApprovalGroup)
                                   .ThenInclude(sg => sg.ApprovalGroups)
                                   .Where(c => !c.IsFinished && !c.IsCanceled && c.StepApprovalGroup.CreatedAt <= beforeTwoDays
                                     && c.StepApprovalGroup.ApprovalGroups.Any(ag => !ag.IsApproved))
                                     .ToListAsync();
        }


        private async Task<List<string>> GetManagerToEscalate(int majorArea, int company, List<int> approvalGroupIds)
        {

            var request = _context.EscalationPointEmployees
                .Include(x => x.ApprovalGroupEmployee)
                .Where(x => x.ApprovalGroupEmployee.MajourAreaId == majorArea
                    && x.ApprovalGroupEmployee.CompanyId == company
                            // && approvalGroupIds.Contains(x.ApprovalGroupEmployee.ApprovalGroupId)
                            );

            var allManagers = await request.ToListAsync();
            List<string> filteredManagers = allManagers
                .Where(x => approvalGroupIds.Contains(x.ApprovalGroupEmployee.ApprovalGroupId))
                .Select(x => x.Email)
                .ToList();
            return filteredManagers;

            //request = request.Where(x => approvalGroupIds.Contains(x.ApprovalGroupEmployee.ApprovalGroupId));


            //return await request.Select(x => x.Email)
            // .ToListAsync();
        }

        #endregion

    }
}
