using ClearanceCycle.Application.Dtos;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Commands
{
    public record PendingClearanceCommand : IRequest<ReponseDto>
    {
        public int RequestId { get; set; }
        public int NextStepId { get; set; }
        public int ApprovalGroupId { get; set; }
        public string ActionBy { get; set; }
        public string Comment { get; set; }
        //public PendingClearanceCommand(int requestId, int nextStepId, int approvalGroupId,string actionBy,string comment)
        //{
        //    RequestId = requestId;
        //    NextStepId = nextStepId;
        //    ApprovalGroupId = approvalGroupId;
        //    ActionBy = actionBy;
        //    Comment = comment;
        //}
    }
}
