using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearanceCycle.Application.Dtos
{
    public record OpenTicketDto
    {
        public string ResigneeHrID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
    }
}
