namespace ClearanceCycle.DataAcess.Implementation
{
    using ApprovalSystem.Services.Services.Interface;
    using ClearanceCycle.Application.Dtos;
    using ClearanceCycle.Application.Interfaces;
    using ClearanceCycle.Application.UseCases.Commands;
    using ClearanceCycle.Domain.Entities;
    using ClearanceCycle.Domain.Enums;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;

    public class WriteRepository : IWriteRepository
    {
        private readonly AuthDbContext _context;
        private readonly IApprovalCycleService _approvalCycleService;
        public WriteRepository(AuthDbContext context, IApprovalCycleService approvalCycleService)
        {
            _context = context;
            _approvalCycleService = approvalCycleService;
            


        }
        #region Add New Clearance Request
        public async Task<int> AddAsync(ClearanceRequest clearanceRequest)
        {
            _context.ClearanceRequests.Add(clearanceRequest);

            return await _context.SaveChangesAsync();


        }
        #endregion

        #region Approve Clearance Request
        public async Task<ReponseDto> ApproveRequest(ProcessClearanceActionCommand approveClearance)
        {

            var step = await _approvalCycleService.GetCurrentStepWithApprovalGroupIds(approveClearance.NextStepId);
            ClearanceRequest? request = await _context.ClearanceRequests
                                                      .Include(c => c.StepApprovalGroup)
                                                      .ThenInclude(s => s.ApprovalGroups)
                                                      .FirstOrDefaultAsync(c => c.Id == approveClearance.RequestId && !c.IsCanceled && !c.IsFinished);

            if (request == null)
            {
                throw new InvalidOperationException("Request Doesn't Exist");
            }

            if (approveClearance.NextStepId == null)
            {
                request.IsFinished = true;
                request.Status = ResignationStatus.Finished;
            }
            var approvalGroupToUpdate = request.StepApprovalGroup.ApprovalGroups
                .FirstOrDefault(a => a.ApprovalGroupId == approveClearance.ApprovalGroupId);

            if (approvalGroupToUpdate != null)
            {
                approvalGroupToUpdate.IsApproved = true;
                request.Status = ResignationStatus.PendingApprove;

            }

            bool allGroupsApproved = request.StepApprovalGroup.ApprovalGroups.All(a => a.IsApproved);

            if (allGroupsApproved || request.StepApprovalGroup.ApprovalGroups.Count == 1)
            {
                request.StepApprovalGroup.IsFinished = true;

                if (approveClearance.NextStepId > 0)
                {
                    var newStep = new StepApprovalGroup
                    {
                        CurrentStepId = approveClearance.NextStepId,
                        IsFinished = false,
                        CreatedAt = DateTime.UtcNow,
                        ApprovalGroups = step.Step.ApprovalGroupIds.Select(id => new StepApprovalAssignments
                        {
                            ApprovalGroupId = id,
                            IsApproved = false
                        }).ToList(),
                        Name = step.Step.Name
                    };

                    _context.StepApprovalGroups.Add(newStep);
                    await _context.SaveChangesAsync();

                    request.StepApprovalGroupId = newStep.Id;
                }
            }

            if (await _context.SaveChangesAsync() > 0)
            {
                return new ReponseDto
                {
                    Message = "Request Approved Successfully",
                    Success = true,
                };
            }

            throw new InvalidOperationException("Failed to Save Request Data");
        }
        #endregion

        #region Pending Clearance Request
        public async Task<ReponseDto> PendingRequest(ProcessClearanceActionCommand pendingClearance)
        {
            ClearanceRequest? request = await _context.ClearanceRequests
                                                      .Include(c => c.StepApprovalGroup)
                                                      .ThenInclude(s => s.ApprovalGroups)
                                                      .FirstOrDefaultAsync(c => c.Id == pendingClearance.RequestId && !c.IsCanceled);

            if (request == null)
            {
                throw new InvalidOperationException("Request Doesn't Exist");
            }

            request.Status = ResignationStatus.Pending;
            request.Comment = pendingClearance.Comment;


            if (await _context.SaveChangesAsync() > 0)
            {
                return new ReponseDto
                {
                    Message = "Request Pending Successfully",
                    Success = true,
                };
            }

            throw new InvalidOperationException("Failed to Save Request Data");

        }


        #endregion

        #region Add History
        public async Task<int> AddHistoryAsync(ClearanceHistory clearanceHistory)
        {
            _context.ClearanceHistories.Add(clearanceHistory);

            return await _context.SaveChangesAsync();


        }
        #endregion

        #region Cancel Request 
        public async Task<ReponseDto> CancelRequest(CancelCleareanceRequestCommand cancelCleareance)
        {
            ClearanceRequest? request = await _context.ClearanceRequests
                                                      .FirstOrDefaultAsync(c => c.Id == cancelCleareance.RequestId && !c.IsCanceled && !c.IsFinished);

            if (request == null)
            {
                throw new InvalidOperationException("Request Doesn't Exist");
            }

            request.Status = ResignationStatus.Canceled;
            request.IsCanceled = true;

            if (await _context.SaveChangesAsync() > 0)
            {
                return new ReponseDto
                {
                    Message = "Request Approved Successfully",
                    Success = true,
                };
            }

            throw new InvalidOperationException("Failed to Save Request Data");
        }

        #endregion

        #region Edit Last Working Date 
        public async Task<ReponseDto> UpdateLastWorkingDate(EditLastWorkingDateCommand editLastWorking)
        {
            ClearanceRequest? request = await _context.ClearanceRequests
                                                      .FirstOrDefaultAsync(c => c.Id == editLastWorking.RequestId && !c.IsCanceled && !c.IsFinished);

            if (request == null)
            {
                throw new InvalidOperationException("Request Doesn't Exist");
            }

            request.LastWorkingDayDate = editLastWorking.LastWorkingDay;

            if (await _context.SaveChangesAsync() > 0)
            {
                return new ReponseDto
                {
                    Message = "Request Updated Successfully",
                    Success = true,
                };
            }

            throw new InvalidOperationException("Failed to Save Request Data");
        }

        #endregion

        public async Task<int> AddApprovalWithEscalationAsync(ApprovalGroupEmployeesDto request)
        {
            if (request == null || request.EscalationEmails == null)
            {
                throw new ArgumentException("Invalid request data.");
            }

            try
            {
                var approvalGroupEmployee = new ApprovalGroupEmployee
                {
                    ApprovalGroupId = request.ApprovalGroupId,
                    EmployeeId = request.EmployeeId,
                    IsActive = request.IsActive,
                    IsAdmin = request.IsAdmin,
                    CompanyId = request.CompanyId,
                    MajorAreaId = request.MajorAreaId
                };

                _context.ApprovalGroupEmployees.Add(approvalGroupEmployee);

                var escalationEmployees = request.EscalationEmails.Select(email => new EscalationPointEmployee
                {
                    ApprovalGroupEmployeeId = approvalGroupEmployee.Id,
                    Email = email
                }).ToList();

                _context.EscalationPointEmployees.AddRange(escalationEmployees);
                await _context.SaveChangesAsync();

                return approvalGroupEmployee.Id;
            }
            catch
            {
                throw;
            }
        }


    }
}
