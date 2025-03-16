using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.Interfaces;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Queries
{
    public class GetClearanceDataTableQueryHandler : IRequestHandler<GetClearanceDataTableQuery, ResultDto<ClearanceRequestsDto>>
    {
        private readonly IReadRepository _readRepository;

        public GetClearanceDataTableQueryHandler(IReadRepository readRepository)
        {
            _readRepository = readRepository;   
        }
        public async Task<ResultDto<ClearanceRequestsDto>> Handle(GetClearanceDataTableQuery request, CancellationToken cancellationToken)
        {
            if (request.IsFinishedRequest) { return await _readRepository.GetAllFinishedRequests(request); }
          return  await _readRepository.GetAllRequests(request);
        }
    }
}
