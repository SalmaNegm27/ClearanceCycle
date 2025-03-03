using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearanceCycle.WorkFlow.DTOs;

namespace ClearanceCycle.Application.Dtos
{
    public record ClearanceDetailsDto
    {
        public int Id { get; set; }
        public string ResigneeHrId { get; set; }
        public string ResigneeName { get; set; }
        public string Reason { get; set; }
        public string LastWorkingDayDate { get; set; }
        public string CreatedAt { get; set; }
        public string StepApprovalGroupName { get; set; }
        public string FunctionName { get; set; }
        public string MajourArea { get; set; }
        public string Department { get; set; }
        public string SubDepartment { get; set; }

        public string Status { get; set; }
        public string Comment { get; set; }

        public string DirectManager { get; set; }

        public IList<ActionDto> Actions { get; set; }
    }
}
