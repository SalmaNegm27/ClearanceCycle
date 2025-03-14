using ApprovalSystem.Services.Services.Interface;
using ClearanceCycle.Application.Interfaces;
using ClearanceCycle.Domain.Factories;
using ClearanceCycle.WorkFlow.Repositories.Interface;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Commands
{
    public class AddClearanceCommandHandler : IRequestHandler<AddClearanceCommand, int>
    {
        private readonly IWriteRepository _writeRepository;
        private readonly IReadRepository _readRepository;
        private readonly IApprovalCycleService _approvalCycle;
        private readonly ICycleRepository _cycleRepository;

        public AddClearanceCommandHandler(IWriteRepository writeRepository, IReadRepository readRepository,IApprovalCycleService approvalCycle, ICycleRepository cycleRepository)
        {
            _writeRepository = writeRepository;
            _approvalCycle = approvalCycle;
            _cycleRepository = cycleRepository;
            _readRepository = readRepository;

        }


        public async Task<int> Handle(AddClearanceCommand request, CancellationToken cancellationToken)
        {
            //get cycle
            var cycleId =_cycleRepository.GetCycleByCompanyId(request.CompanyId);
            //get first Step
            WorkFlow.DTOs.StepResponseDto firstStep = await _approvalCycle.GetFirstStep(cycleId);
            if (firstStep == null) throw new Exception("Invalid cycle step data.");

            if (await _readRepository.ExistsAsync(request.ResigneeId))
            {
                throw new InvalidOperationException("Clearance request already exists.");
            }

            var clearance = ClearanceFactory.Create(request.CompanyId, request.ResigneeId, request.LastWorkingDay, request.ResignationReasonId, request.ResigneeHrId, request.ResigneeName, request.CreatedBy
                ,firstStep.Step.Id,firstStep.Step.ApprovalGroupIds, firstStep.Step.Name,request.CompanyName,request.DirectManagerHrId,request.SecondManagerHrId
                );
            await _writeRepository.AddAsync(clearance);
            return clearance.Id;
        }

       
    }
}
