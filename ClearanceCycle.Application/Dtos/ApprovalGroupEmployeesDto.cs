using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearanceCycle.Application.Dtos
{
    public class ApprovalGroupEmployeesDto
    {
        public int ApprovalGroupId { get; set; }
        public int EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public int? CompanyId { get; set; }
        public int? MajorAreaId { get; set; }
        public List<string> EscalationEmails { get; set; } = new();
    }

}

