using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.Interfaces;
using ClearanceCycle.Domain.Factories;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Commands
{
    public class EditLastWorkingDateCommandHandler : IRequestHandler<EditLastWorkingDateCommand, ReponseDto>
    {
        private readonly IWriteRepository _writeRepository;
        public EditLastWorkingDateCommandHandler(IWriteRepository writeRepository)
        {
            _writeRepository = writeRepository;
        }
        public async Task<ReponseDto> Handle(EditLastWorkingDateCommand request, CancellationToken cancellationToken)
        {
          var result =  await _writeRepository.UpdateLastWorkingDate(request);
            var requestHistory = ClearanceHistoryFactory.Create(request.ActionBy, Domain.Enums.ActionType.Modified, "Request Updated", request.RequestId,"");
            await _writeRepository.AddHistoryAsync(requestHistory);
            return result;
        }
    }
}
