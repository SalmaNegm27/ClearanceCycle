using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.Interfaces;
using ClearanceCycle.Domain.Factories;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Commands
{
    public class CancelClearanceRequestHandler : IRequestHandler<CancelCleareanceRequestCommand, ReponseDto>
    {
        private readonly IWriteRepository _writeRepository;
        private readonly IReadRepository _readRepository;


        public CancelClearanceRequestHandler(IWriteRepository writeRepository, IReadRepository readRepository)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
        }
        public async Task<ReponseDto> Handle(CancelCleareanceRequestCommand request, CancellationToken cancellationToken)
        {
            var groupName = await _readRepository.GetGroupName(request.ApprovalGroupId);

            var cancelRequest = await _writeRepository.CancelRequest(request);
            var requestHistory = ClearanceHistoryFactory.Create(request.ActionBy, Domain.Enums.ActionType.Canceled, "Request Canceled", request.RequestId,groupName);
            await _writeRepository.AddHistoryAsync(requestHistory);
            return cancelRequest;
        }
    }
}
