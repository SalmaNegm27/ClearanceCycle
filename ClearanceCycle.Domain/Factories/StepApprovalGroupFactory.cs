using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearanceCycle.Domain.Entities;
using ClearanceCycle.Domain.Enums;

namespace ClearanceCycle.Domain.Factories
{
    public class StepApprovalGroupFactory
    {
        public static StepApprovalGroup Create(int currentStepId, bool isFinished,List<int> approvalGroups,string stepName)
        {
            if (currentStepId <= 0)
            {
                throw new ArgumentException("Invalid systemId", nameof(currentStepId));
            }
            return new StepApprovalGroup
            {
                CurrentStepId = currentStepId,
                IsFinished = isFinished,
                ApprovalGroups = approvalGroups.Select(id => new StepApprovalGroupApproval
                {
                    ApprovalGroupId = id
                })
                .ToList(),
                Name = stepName

            };
        }
    }
}
