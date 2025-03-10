namespace ClearanceCycle.Domain.Entities
{
    public class StepApprovalGroup
    {
        public int Id { get; set; }
        public int? CurrentStepId { get; set; }
        public bool IsFinished { get; set; }
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<StepApprovalAssignments> ApprovalGroups { get; set; } = new List<StepApprovalAssignments>();

        public ICollection<ClearanceRequest> ClearanceRequests { get; set; }
    }
}
