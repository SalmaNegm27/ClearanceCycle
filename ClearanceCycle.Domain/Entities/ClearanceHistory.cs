using ClearanceCycle.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClearanceCycle.Domain.Entities
{
    public class ClearanceHistory
    {
        public int Id { get; set; }
        public int ClearanceRequestId { get; set; }
        public ClearanceRequest ClearanceRequest { get; set; }
        [MaxLength(10)]

        public string ActionBy { get; set; }
        public DateTime ActionAt { get; set; } = DateTime.UtcNow;
        [MaxLength(200)]

        public string Comment { get; set; }

        [MaxLength(50)]
        public string ApprovalGroup { get; set; }

        public ActionType ActionType { get; set; }
      
    }
}
