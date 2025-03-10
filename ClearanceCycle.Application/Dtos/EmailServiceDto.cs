using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearanceCycle.Application.Dtos
{
    public class EmailAddress
    {
        public string Address { get; set; }
    }

    public class Email
    {
        public string HtmlBody { get; set; }
        public string Subject { get; set; }
        public List<Recipient> To { get; set; }
    }

    public class Recipient
    {
        public EmailAddress EmailAddress { get; set; }
    }

    public class EmailServiceDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Email Email { get; set; }
    }
}
