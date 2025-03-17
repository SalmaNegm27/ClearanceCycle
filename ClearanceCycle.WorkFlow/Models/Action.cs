using System.ComponentModel.DataAnnotations;

namespace ClearanceCycle.WorkFlow.Models
{
    public class Action
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; }
        public IList<StepAction>? StepActions { get; set; }
    }
}
