using ClearanceCycle.Application.Dtos;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Commands
{
    public record ProcessClearanceActionCommand : IRequest<ReponseDto>
    {
        public int RequestId { get; set; }
        public int NextStepId { get; set; }
        public int ApprovalGroupId { get; set; }
        public string ActionBy { get; set; }
        public int ActionId { get; set; }

        public string? Comment { get; set; }
    }

}
