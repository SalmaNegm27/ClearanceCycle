using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearanceCycle.Application.Dtos;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Commands
{
    public record CancelCleareanceRequestCommand : IRequest<ReponseDto>
    {
        public int RequestId { get; set; }
        public string ActionBy { get; set; }
        public CancelCleareanceRequestCommand(int requstId, string actionBy)
        {
            RequestId = requstId;
            ActionBy = actionBy;
        }
    }
}
