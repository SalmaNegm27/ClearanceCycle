using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearanceCycle.DataAcess.HangFireService
{
   public interface IHangFireService
    {
        Task<bool> DeactivateAllEmployeeAccounts();
        Task ProcessEscalation();
    }
}
