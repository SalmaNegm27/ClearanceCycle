using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearanceCycle.WorkFlow.Models
{
    public class Action
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; }
        public IList<StepAction>? StepActions { get; set; }
    }
}
