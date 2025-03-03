using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearanceCycle.Domain.Entities;

namespace ClearanceCycle.Domain.Factories
{
    public class ClearanceStepStatusFactory
    {
        public static ClearanceStepStatus Create(int requestId,int approvalgroupId)
        {
            return new ClearanceStepStatus()
            {
                Comment ="",
                RequestId = requestId,
                ApprovalGroupId = approvalgroupId,
                Status ="Pending",
                

            };
            }
    }
}
