using ClearanceCycle.Application.Dtos;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Commands
{
    public record ApproveClearanceCommand : IRequest<ReponseDto>
    {
        public int RequestId { get; set; }
        public int NextStepId { get; set; }
        public int ApprovalGroupId { get; set; }
        public string ActionBy { get; set; }
        public int ActionId { get; set; }

        //public ApproveClearanceCommand(int nextStepId , int approvalGroupId,int requestId,string actionBy,int actionId)
        //{
        //    RequestId = requestId;
        //    NextStepId = nextStepId;
        //    ApprovalGroupId = approvalGroupId;
        //    ActionBy = actionBy;
        //   ActionId = actionId;
        //}
    }
}
