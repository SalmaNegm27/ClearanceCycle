using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.Interfaces;
using ClearanceCycle.Application.UseCases.Commands;
using ClearanceCycle.DataAcess.Models;
using ClearanceCycle.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClearanceCycle.DataAcess.Implementation
{
    public class WriteRepository : IWriteRepository
    {
        private readonly AuthDbContext _context;
        public WriteRepository(AuthDbContext context)
        {
            _context = context;
        }
        #region Add New Clearance Request
        public async Task<int> AddAsync(ClearanceRequest clearanceRequest)
        {
            _context.ClearanceRequests.Add(clearanceRequest);

            return await _context.SaveChangesAsync();


        }
        #endregion

        #region Approve Clearance Request
        public async Task<ReponseDto> ApproveRequest(ProcessClearanceActionCommand approveClearance, List<int> approvalGroups, string stepName)
        {
            ClearanceRequest? request = await _context.ClearanceRequests
                                                      .Include(c => c.StepApprovalGroup)
                                                      .ThenInclude(s => s.ApprovalGroups)
                                                      .FirstOrDefaultAsync(c => c.Id == approveClearance.RequestId && !c.IsCanceled && !c.IsFinished);

            if (request == null)
            {
                throw new InvalidOperationException("Request Doesn't Exist");
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
                        ApprovalGroups = approvalGroups.Select(id => new StepApprovalGroupApproval
                        {
                            ApprovalGroupId = id,
                            IsApproved = false
                        }).ToList(),
                        Name = stepName
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

    }
}
