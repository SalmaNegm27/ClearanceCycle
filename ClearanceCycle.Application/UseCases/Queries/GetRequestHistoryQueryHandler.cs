using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.Interfaces;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Queries
{
    
    public class GetRequestHistoryQueryHandler : IRequestHandler<GetRequestHistoryQuery, ResultDto<RequestHistoryDto>>
    {
        private readonly IReadRepository _readRepository;
        public GetRequestHistoryQueryHandler(IReadRepository readRepository)
        {
            _readRepository = readRepository;
        }
        public async Task<ResultDto<RequestHistoryDto>> Handle(GetRequestHistoryQuery request, CancellationToken cancellationToken)
        {
            return await _readRepository.GetAllRequestHistory(request);
        }
    }
}
