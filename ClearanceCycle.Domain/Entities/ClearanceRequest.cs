using ClearanceCycle.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClearanceCycle.Domain.Entities
{
    public class ClearanceRequest
    {
        public int Id { get; set; }
        
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [MaxLength(20)]
        public string ResigneeHrId { get; set; }
        [MaxLength(30)]
        public string ResigneeName { get; set; }
        public int ClearanceReasonId { get;  set; }
        public ClearanceReason ClearanceReason { get; set; }
        public ResignationStatus? Status { get; set; }
        [MaxLength(50)]
        public string? ResignationFileName { get; set; }
        public DateTime LastWorkingDayDate { get; set; } = DateTime.UtcNow;
        [MaxLength(50)]
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public bool IsFinished { get; set; }
        public bool IsCanceled { get; set; }
        public string DirectManagerHrid { get; set; }
        public string SecondManagerHrId { get; set; }
        [MaxLength(10)]
        public string CompanyName { get; set; }
        public int StepApprovalGroupId { get; set; }
        public StepApprovalGroup StepApprovalGroup { get; set; }
        public ICollection<ClearanceHistory>? ClearanceHistories { get; set; }

        public bool IsResigneeAccountsClosed { get; set; }
        public DateTime AccountsClosedAt { get; set; } = DateTime.Now;


    }
  
}
