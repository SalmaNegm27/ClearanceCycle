using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearanceCycle.WorkFlow.DTOs
{
    public class BaseResponseDto
    {
        public bool Success { get; set; } = false;
        public bool IsCompleted { get; set; } = false;
        public string? Message { get; set; }
    }
    public class StepResponseDto : BaseResponseDto
    {
        public StepDto? Step { get; set; }
    }
    public class ActionResponseDto : BaseResponseDto
    {
        public List<ActionDto> Actions { get; set; }
    }
    public class StepDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> ApprovalGroupIds { get; set; }
    }
    public class ActionDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? NextStepId { get; set; }
        public bool NeedComment { get; set; } = false;

    }


}
