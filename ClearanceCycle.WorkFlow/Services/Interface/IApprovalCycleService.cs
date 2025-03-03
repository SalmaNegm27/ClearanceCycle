using ClearanceCycle.WorkFlow.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApprovalSystem.Services.Services.Interface
{
    public interface IApprovalCycleService
    {
        public Task<StepResponseDto> GetFirstStep( int cycleId);
        public Task<StepResponseDto> GetCurrentStepWithApprovalGroupIds(int stepId);
        public Task<ActionResponseDto> GetCurrentStepActions(int stepId);
    }
}
