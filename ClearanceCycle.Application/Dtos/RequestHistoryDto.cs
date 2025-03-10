using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearanceCycle.Application.Dtos
{
    public record RequestHistoryDto
    {
        public string ActionBy { get; set; }
        public string ActionAt { get; set; }
        public string ActionType { get; set; }
        public string Comment { get; set; }
        public string GroupName { get; set; }
        
    }
}
