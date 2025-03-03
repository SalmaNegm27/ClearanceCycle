using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearanceCycle.Domain.Entities
{
    public class StepApprovalGroupApproval
    {
        public int Id { get; set; }
        public int StepApprovalGroupId { get; set; }
        public StepApprovalGroup StepApprovalGroup { get; set; }

        public bool IsApproved { get; set; }
        public int ApprovalGroupId { get; set; }
    }
}
