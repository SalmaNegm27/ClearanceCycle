namespace ClearanceCycle.WorkFlow.Models
{
    public class StepAction
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public Action? Action { get; set; }
        public int CurrentStepId { get; set; }
        public Step CurrentStep { get; set; }
        public int? NextStepId { get; set; }
        public bool NeedComment { get; set; }

    }
}
