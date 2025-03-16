using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearanceCycle.Application.Dtos;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Queries
{
    public class GetClearanceDataTableQuery :IRequest<ResultDto<ClearanceRequestsDto>>
    {
        public int PageNumber { get; set; }
        public int RawsNumber { get; set; }
        public string? SortField { get; set; }
        public string? SortType { get; set; }
        public string? Filter { get; set; }
        public int? GroupId { get; set; }
        public string? HrId { get; set; }

        public bool IsFinishedRequest { get; set; }

    }
}
