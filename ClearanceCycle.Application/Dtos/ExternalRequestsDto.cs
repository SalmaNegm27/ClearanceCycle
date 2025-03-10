using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearanceCycle.Application.Dtos
{
   public class ExternalRequestsDto
    {
        public string BaseUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SystemRequestTokenDto
    {
        public ExternalRequestsDto PortalSettings { get; set; }
        public ExternalRequestsDto RetailApiSettings { get; set; }
        public ExternalRequestsDto ExternalIntegration { get; set; }
        public ExternalRequestsDto MyAman { get; set; }
       

    }

    public class TokenSettingResponse
    {
        public bool IsSuccess { get; set; }
        public RmsTokenResponse data { get; set; }
        public string Message { get; set; }
        public string token { get; set; }

    }

    public class RmsTokenResponse
    {
        public string accessToken { get; set; }
    }
}
