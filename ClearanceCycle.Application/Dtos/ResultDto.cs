using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearanceCycle.Application.Dtos
{
    public class ReponseDto
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; }
    }
    public class ResultDto <T> : ReponseDto
    {
        public object Data { get; set; } 
        
        public int? TotalRecords { get; set; }
    }
}
