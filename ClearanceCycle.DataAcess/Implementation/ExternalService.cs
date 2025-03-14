
namespace ClearanceCycle.DataAcess.Implementation
{
    using ClearanceCycle.Application.Dtos;
    using Microsoft.Extensions.Configuration;

    public class ExternalService : IExternalService
    {
        private readonly ExternalApilLogger _client;
        private readonly SystemRequestTokenDto _settings;

        public ExternalService(IOptions<SystemRequestTokenDto> settings, ExternalApilLogger client)
        {
            _client = client;
            _settings = settings.Value;

        }
        public async Task<string> GetRMSToken()
        {
            var credential = _settings.RetailApiSettings;
            string url = $"{credential.BaseUrl}/InActiveCashierLogin";

            var obj = new
            {
                userID = credential.Username,
                password = credential.Password
            };

            var respone = await _client.SendAsync<TokenSettingResponse>(url, HttpMethod.Post, obj);
            if (respone.IsSuccess)
            {
                return respone.data.accessToken;
            }

            return null;

        }

        public async Task<string> GetPortalToken()
        {
            var credential = _settings.PortalSettings;
            string url = $"{credential.BaseUrl}/api/Auth/LoginByUserName";

            var obj = new
            {
                userName = credential.Username,
                password = credential.Password
            };
            var respone = await _client.SendAsync<TokenSettingResponse>(url, HttpMethod.Post, obj);
            if (respone.IsSuccess)
            {
                return respone.token;
            }

            return null;

        }

        public async Task<bool> DeactivateEmployeeAmanCardAPI(string NationalId)
        {
            var credential = _settings.ExternalIntegration;
            string url = $"{credential.BaseUrl}/Card/DeactivateEmployeeAmanCard";

            var param = new { Lang = 1, NationalId = NationalId };
            var res = await _client.SendAsync<ResMsgRespones>(url, HttpMethod.Post, param);
            if (res.ResultID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeactivateEmployeePortalAccounts(string hr_id)
        {
            var credential = _settings.PortalSettings;
            var token = await GetPortalToken();
            if (token == null)
            {
                return false;
            }
            string url = $"{credential.BaseUrl}/api/User/DeleteUser";
            var obj = new
            {
                userHrid = hr_id

            };
            var respone = await _client.SendAsync<TokenSettingResponse>(url, HttpMethod.Post, obj, token);

            if (respone.Message == "Success" || respone.IsSuccess || respone.Message.Trim() == "Error On Deleting User, UserHRID doesn't exist")
            {
                return true;
            }
            return false;
        }
        public async Task<bool> DeactivateEmployeeBusinessAccount(string hr_id)
        {
            var token = await GetRMSToken();
            if (token == null)
            {
                return false;
            }
            var credential = _settings.RetailApiSettings;
            string url = $"{credential.BaseUrl}/UpdateCashierActivation";
            var obj = new
            {
                hrid = hr_id

            };
            var respone = await _client.SendAsync<ReponseDto>(url, HttpMethod.Post, obj, token);

            if (/*response.IsSuccessful*/  respone.Message == "User Now InActive")
            {
                return true;
            }


            return false;


        }
        public async Task SendMails(EmailServiceDto emailService)
        {
            var credential = _settings.MyAman;
            string url = $"{credential.BaseUrl}/AmanMail/api/Mail/ZohoSend";
            var obj = new
            {
                username = credential.Username,
                password = credential.Password,
                email = emailService.Email
            };
            await _client.SendAsync<string>(url, HttpMethod.Post, obj);

        }

        public async Task<bool> ActiveEmployeePortalAccount(string hr_id)
        {
            var response = new TokenSettingResponse();
            var credential = _settings.PortalSettings;
            var token = await GetPortalToken();
            if (token == null)
            {
               return false;
            }
            string url = $"{credential.BaseUrl}/api/User/ActivateUser";
            var obj = new
            {
                userHrid = hr_id

            };
            try
            {
                var respone = await _client.SendAsync<TokenSettingResponse>(url, HttpMethod.Post, obj, token);

                if (respone.IsSuccess)
                {
                   return true ;
                }
                
                return false;

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to Active user in portal" + ex.Message);
            }
        }



    }
}
