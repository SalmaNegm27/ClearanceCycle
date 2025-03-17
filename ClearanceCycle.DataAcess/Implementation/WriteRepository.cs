namespace ClearanceCycle.DataAcess.Implementation
{
    using ApprovalSystem.Services.Services.Interface;
    using Azure;
    using Azure.Core;
    using ClearanceCycle.Application.Dtos;
    using ClearanceCycle.Application.Interfaces;
    using ClearanceCycle.Application.UseCases.Commands;
    using ClearanceCycle.Domain.Entities;
    using ClearanceCycle.Domain.Enums;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;

    public class WriteRepository : IWriteRepository
    {
        private readonly AuthDbContext _context;
        private readonly IApprovalCycleService _approvalCycleService;
        private static readonly string[] AllowedExtensions = { ".JPEG", ".JPG", ".PNG", ".PDF" };
        private readonly IExternalService _externalService;

        public WriteRepository(AuthDbContext context, IApprovalCycleService approvalCycleService, IExternalService externalService)
        {
            _context = context;
            _approvalCycleService = approvalCycleService;
            _externalService = externalService;
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

            ClearanceRequest? request = await _context.ClearanceRequests
                                                      .Include(c => c.StepApprovalGroup)
                                                      .ThenInclude(s => s.ApprovalGroups)
                                                      .FirstOrDefaultAsync(c => c.Id == approveClearance.RequestId && !c.IsCanceled && !c.IsFinished);

            if (request == null)
            {
                throw new InvalidOperationException("Request Doesn't Exist");
            }

            var approvalGroups = request.StepApprovalGroup?.ApprovalGroups;
            if (approvalGroups == null)
                throw new InvalidOperationException("Approval groups not found.");

            var approvalGroupToUpdate = approvalGroups.FirstOrDefault(a => a.ApprovalGroupId == approveClearance.ApprovalGroupId);
            if (approvalGroupToUpdate == null)
                throw new InvalidOperationException("Approval group not found.");

            if (!approvalGroupToUpdate.IsApproved)
            {
                approvalGroupToUpdate.IsApproved = true;
                request.Status = ResignationStatus.PendingApprove;
            }


            bool allGroupsApproved = approvalGroups.All(a => a.IsApproved);

            if (allGroupsApproved || approvalGroups.Count == 1)
            {
                request.StepApprovalGroup.IsFinished = true;

                if (approveClearance.NextStepId.HasValue)
                {
                    var step = await _approvalCycleService.GetCurrentStepWithApprovalGroupIds(approveClearance.NextStepId);

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
                else
                {
                    request.IsFinished = true;
                    request.Status = ResignationStatus.Finished;
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
        public async Task<ReponseDto> CancelRequest(ProcessClearanceActionCommand cancelCleareance)
        {

            var request = await GetCleranceQuery(cancelCleareance.RequestId);

            if (request.IsCanceled)
            {
                return new ReponseDto
                {
                    Message = "request Already Canceled",
                    Success = false,
                };
            }
            request.Status = ResignationStatus.Canceled;
            request.IsCanceled = true;
            request.Employee.Active = true;

            if (await _context.SaveChangesAsync() < 0)
            {
                throw new InvalidOperationException("Failed to Save Request Data");
            }
            OpenTicketDto ticketDto = new OpenTicketDto
            {
                ResigneeHrID = request.ResigneeHrId,
                Subject = "Request to Open RmS Accounts ",
                Description = $"request for activation  accounts for RMS for this employee {request.ResigneeHrId}. Could you please assist in setting up the necessary accounts"
            };

            var response = await _externalService.OpenTicketAsync(ticketDto);
            if (response.Success)
            {
                return new ReponseDto
                {
                    Message = "request Canceled Successfully",
                    Success = true,
                };
            }

            throw new InvalidOperationException("Failed to Save Cancel Request Data");
        }

        #endregion

        #region Edit Last Working Date 
        public async Task<ReponseDto> UpdateLastWorkingDate(EditLastWorkingDateCommand editLastWorking)
        {
            ClearanceRequest? request = await GetCleranceQuery(editLastWorking.RequestId);


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
                    //MajourAreaId = request.MajorAreaId,
                    EscalationPointEmployees = request.EscalationEmails.Select(email => new EscalationPointEmployee
                    {
                        //ApprovalGroupEmployeeId = approvalGroupEmployee.Id,
                        Email = email
                    }).ToList()
                };

                _context.ApprovalGroupEmployees.Add(approvalGroupEmployee);



                //_context.EscalationPointEmployees.AddRange(approvalGroupEmployee);
                await _context.SaveChangesAsync();

                return approvalGroupEmployee.Id;
            }
            catch
            {
                throw;
            }
        }


        public async Task<ReponseDto> UploadClearanceFile(UploadDocumentCommand uploadDocument)
        {
            var result = new ReponseDto();
            var request = await GetCleranceQuery(uploadDocument.RequestId);
            var res = await UploadedFile(uploadDocument.File);
            if (res == null)
            {
                result.Success = false;
                result.Message = "failed to  uploaded file";
            }

            request.ResignationFileName = res;
            var saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                result.Success = true;
                result.Message = "File uploaded successfully";
                return result;
            }
            return result;
        }
        private async Task<string> UploadedFile(IFormFile file)
        {

            if (file == null || file.Length == 0)
            {
                throw new ArgumentNullException(nameof(file), "File is empty or null.");
            }

            string fileExtension = Path.GetExtension(file.FileName).ToUpper();

            if (!AllowedExtensions.Contains(fileExtension))
            {
                return null;
            }

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Resignations");
            string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);


            if (!Path.GetFullPath(filePath).StartsWith(Path.GetFullPath(uploadsFolder)))
            {
                throw new InvalidOperationException("Invalid file path.");
            }

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(stream);
            }

            return uniqueFileName;
        }

        private async Task<ClearanceRequest> GetCleranceQuery(int requestId)
        {
            var request = await _context.ClearanceRequests.Include(x => x.Employee)
                                             .FirstOrDefaultAsync(c => c.Id == requestId  && !c.IsFinished);

            if (request == null)
            {
                throw new InvalidOperationException("Request Doesn't Exist");
            }

            return request;

        }

    }
}
