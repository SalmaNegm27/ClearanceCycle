namespace ClearanceCycle.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class ClearanceReason
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Reason { get; set; }
    }
}
