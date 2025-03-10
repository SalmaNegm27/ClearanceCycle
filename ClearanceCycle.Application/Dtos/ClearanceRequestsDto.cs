using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearanceCycle.Application.Dtos
{
    public class ClearanceRequestsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HrId { get; set; }
        public string Department { get; set; }
        public string Company { get; set; }
        public string Reason { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
        public string CurrentStepName { get; set; }
        public string LastWorkingDayDate { get; set; }
        public int? currentStepId { get; set; }
    }
}
