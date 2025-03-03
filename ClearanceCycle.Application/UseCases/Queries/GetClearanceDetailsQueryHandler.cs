using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.Interfaces;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Queries
{
    public record GetClearanceDetailsQueryHandler :IRequestHandler<GetClearanceDetailsQuery, ResultDto<ClearanceDetailsDto>>
    {
        private readonly IReadRepository _readRepository;

        
        public GetClearanceDetailsQueryHandler(IReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<ResultDto<ClearanceDetailsDto>> Handle(GetClearanceDetailsQuery request, CancellationToken cancellationToken)
        {
          
           return await _readRepository.GetRequestByID(request.Id,request.StepId);
        }
    }
}
