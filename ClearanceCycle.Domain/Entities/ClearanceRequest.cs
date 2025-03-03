using System.ComponentModel.DataAnnotations.Schema;

namespace ClearanceCycle.Domain.Entities
{
    public class ClearanceRequest
    {
        public int Id { get; set; }
        
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string ResigneeHrId { get; set; }
        public string ResigneeName { get; set; }
        public int ClearanceReasonId { get;  set; }
        public ClearanceReason ClearanceReason { get; set; }
        public ResignationStatus? Status { get; set; }
        public string? ResignationFileName { get; set; }
        public DateTime LastWorkingDayDate { get; set; } = DateTime.Now;
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public bool IsFinished { get; set; }
        public bool IsCanceled { get; set; }
        public string DirectManagerHrid { get; set; }
        public string DepartmentManagerHrid { get; set; }
        public string CompanyName { get; set; }
    
        public int StepApprovalGroupId { get; set; }
        public StepApprovalGroup StepApprovalGroup { get; set; }


        public ICollection<ClearanceHistory>? ClearanceHistories { get; set; }


    }
    public enum ResignationStatus
    {
        New=1,
        Pending=2,
        PendingApprove=3,
        Canceled=4,
        Finished=5

    }
}
