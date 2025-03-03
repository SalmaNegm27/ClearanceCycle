using ClearanceCycle.Application.Dtos;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Queries
{
    public record GetRequestHistoryQuery :IRequest<ResultDto<RequestHistoryDto>>
    {
        public int RequestId { get; set; }
        public GetRequestHistoryQuery(int requestId)
        {
            RequestId = requestId;
        }
    }
}
