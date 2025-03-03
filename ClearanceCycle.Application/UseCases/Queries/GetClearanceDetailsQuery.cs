using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearanceCycle.Application.Dtos;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Queries
{
    public class GetClearanceDetailsQuery :IRequest<ResultDto<ClearanceDetailsDto>>
    {
        public int Id { get; set; }
        public int StepId { get; set; }
        public GetClearanceDetailsQuery(int requestId,int currentStepId)
        {
            Id = requestId; 
            StepId = currentStepId;
        }
    }
}
