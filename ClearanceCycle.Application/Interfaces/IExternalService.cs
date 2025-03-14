using ClearanceCycle.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearanceCycle.Application.Interfaces
{
    public interface IExternalService
    {
        Task<string> GetPortalToken();
        Task<bool> DeactivateEmployeePortalAccounts(string hr_id);
        Task<bool> DeactivateEmployeeAmanCardAPI(string NationalId);
        Task<string> GetRMSToken();
        Task<bool> DeactivateEmployeeBusinessAccount(string hr_id);
        Task SendMails(EmailServiceDto emailService);
        Task<bool> ActiveEmployeePortalAccount(string hr_id);

    }
}
