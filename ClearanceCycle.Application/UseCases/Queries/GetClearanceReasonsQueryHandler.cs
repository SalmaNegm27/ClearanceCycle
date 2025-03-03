using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.Interfaces;
using ClearanceCycle.Domain.Entities;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Queries
{
    public class GetClearanceReasonsQueryHandler : IRequestHandler<GetClearanceReasonsQuery, ResultDto<ClearanceReason>>
    {
       private readonly IReadRepository _readRepository;
        public GetClearanceReasonsQueryHandler(IReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<ResultDto<ClearanceReason>> Handle(GetClearanceReasonsQuery request, CancellationToken cancellationToken)
        {
           return await _readRepository.GetReasons();
        }
    }
}
