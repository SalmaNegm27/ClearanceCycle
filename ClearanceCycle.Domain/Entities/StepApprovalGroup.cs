namespace ClearanceCycle.Domain.Entities
{
    public class StepApprovalGroup
    {
        public int Id { get; set; }
        public int CurrentStepId { get; set; }
        public bool IsFinished { get; set; }
        public string Name { get; set; }
        public ICollection<StepApprovalGroupApproval> ApprovalGroups { get; set; } = new List<StepApprovalGroupApproval>();

        public ICollection<ClearanceRequest> ClearanceRequests { get; set; }
    }
}
