using System.ComponentModel.DataAnnotations;

namespace ClearanceCycle.WorkFlow.Models
{
    public class Step  
    {
        public int Id { get; set; }
        [MaxLength(300)]
        public List<int> ApprovalGroupIds { get; set; } 
        [MaxLength(50)]
        public string Name { get; set; }
        //public int Order { get; set; }
        public int CycleId { get; set; }
        public Cycle? Cycle { get; set; }
        public bool IsParallel { get; set; } = false;

        public IList<StepAction>? StepActions { get; set; }
    }
}
