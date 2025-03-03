using ClearanceCycle.Domain.Enums;

namespace ClearanceCycle.Domain.Entities
{
    public class ClearanceHistory
    {
        public int Id { get; set; }
        public int ClearanceRequestId { get; set; }
        public ClearanceRequest ClearanceRequest { get; set; }
        public string ActionBy { get; set; }
        public DateTime ActionAt { get; set; } = DateTime.Now;
        public string Comment { get; set; }

        public ActionType ActionType { get; set; }
      
    }
}
