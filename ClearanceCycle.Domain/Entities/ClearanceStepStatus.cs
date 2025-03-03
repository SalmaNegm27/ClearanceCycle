namespace ClearanceCycle.Domain.Entities
{
    public class ClearanceStepStatus
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public ClearanceRequest Request { get; set; }
        public int StepId { get; set; }
        public ClearanceStep Step { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Approved
        public int ApprovalGroupId { get; set; } 
        public ApprovalGroup ApprovalGroup { get; set; } 
        public string? Comment { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
