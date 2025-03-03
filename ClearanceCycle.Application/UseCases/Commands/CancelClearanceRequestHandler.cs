using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.Interfaces;
using ClearanceCycle.Domain.Factories;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Commands
{
    public class CancelClearanceRequestHandler : IRequestHandler<CancelCleareanceRequestCommand, ReponseDto>
    {
        private readonly IWriteRepository _writeRepository;

        public CancelClearanceRequestHandler(IWriteRepository writeRepository)
        {
            _writeRepository = writeRepository;
        }
        public async Task<ReponseDto> Handle(CancelCleareanceRequestCommand request, CancellationToken cancellationToken)
        {
            var cancelRequest = await _writeRepository.CancelRequest(request);
            var requestHistory = ClearanceHistoryFactory.Create(request.ActionBy, Domain.Enums.ActionType.Canceled, "Request Canceled", request.RequestId);
            await _writeRepository.AddHistoryAsync(requestHistory);
            return cancelRequest;
        }
    }
}
