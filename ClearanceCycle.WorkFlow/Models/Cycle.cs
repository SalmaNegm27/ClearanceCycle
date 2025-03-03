using System.ComponentModel.DataAnnotations;

namespace ClearanceCycle.WorkFlow.Models
{
    public class Cycle
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; }
        public bool IsActive { get; set; }

        public IList<Step>? Steps { get; set; }
    }
}
