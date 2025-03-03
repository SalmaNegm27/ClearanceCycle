using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearanceCycle.Domain.Entities
{
    public class ClearanceStep
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        public bool IsParallel { get; set; }
        public int StepOrder { get; set; }
        public int CycleId { get; set; }

        public int ApprovalGroupId { get; set; }

    }
}
