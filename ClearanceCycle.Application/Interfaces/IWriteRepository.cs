using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.UseCases.Commands;
using ClearanceCycle.Domain.Entities;

namespace ClearanceCycle.Application.Interfaces
{
    public interface IWriteRepository 
    {
        Task<int> AddAsync(ClearanceRequest clearanceRequest);
        Task<int> AddHistoryAsync(ClearanceHistory clearanceHistory);
        Task<ReponseDto> ApproveRequest(ProcessClearanceActionCommand approveClearance, List<int> approvalGroups, string stepName);
        Task<ReponseDto> PendingRequest(ProcessClearanceActionCommand pendingClearance);

        Task<ReponseDto> CancelRequest(CancelCleareanceRequestCommand cancelCleareance);
        Task<ReponseDto> UpdateLastWorkingDate(EditLastWorkingDateCommand editLastWorking);


    }
}
