namespace ClearanceCycle.WorkFlow.Models
{
    public class StepTransition
    {
        public int Id { get; set; }

        public int StepActionId { get; set; }
        public StepAction StepAction { get; set; }
        public int? NextStepId { get; set; }
        public Step NextActionStep { get; set; }

    }
}