using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearanceCycle.Application.Dtos
{
    public class ResMsgRespones
    {
        public int ResultID { get; set; }
        public string? ResultMessage { get; set; }
        public string Token { get; set; }
    }
}
