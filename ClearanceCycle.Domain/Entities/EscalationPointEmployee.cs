using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [MaxLength(20)]

        public string Email { get; set; }
    }
}
