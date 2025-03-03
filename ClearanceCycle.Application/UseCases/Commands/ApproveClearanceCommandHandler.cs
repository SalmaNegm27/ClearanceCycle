using ApprovalSystem.Services.Services.Interface;
using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.Interfaces;
using ClearanceCycle.Domain.Factories;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Commands
{
    //public class ApproveClearanceCommandHandler : IRequestHandler<ApproveClearanceCommand, ReponseDto>
    //{
    //    private readonly IWriteRepository _writeRepository;
    //    private readonly IApprovalCycleService _approvalCycleService;
    //    public ApproveClearanceCommandHandler(IWriteRepository writeRepository, IApprovalCycleService approvalCycleService)
    //    {
    //        _writeRepository = writeRepository;
    //        _approvalCycleService = approvalCycleService;
    //    }
    //    public async Task<ReponseDto> Handle(ApproveClearanceCommand request, CancellationToken cancellationToken)
    //    {
    //        var step = await _approvalCycleService.GetCurrentStepWithApprovalGroupIds(request.NextStepId);
    //        var approve = await _writeRepository.ApproveRequest(request, step.Step.ApprovalGroupIds, step.Step.Name);
    //        var requestHistory = ClearanceHistoryFactory.Create(request.ActionBy, Domain.Enums.ActionType.Approved, "Request Approved", request.RequestId);
    //        await _writeRepository.AddHistoryAsync(requestHistory);
    //        return approve;


    //    }
    //}
}
