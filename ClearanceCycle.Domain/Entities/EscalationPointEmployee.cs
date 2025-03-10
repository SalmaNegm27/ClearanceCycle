using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearanceCycle.Domain.Entities
{
    public class EscalationPointEmployee
    {
        public int Id { get; set; }
        public int ApprovalGroupEmployeeId { get; set; }
        public ApprovalGroupEmployee ApprovalGroupEmployee { get; set; }
        public string Email { get; set; }
    }
}
