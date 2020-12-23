using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.HandleException
{
    public class SendEmailException : Exception
    {
        public SendEmailException(string message = "Không gửi được email") : base(message)
        {
        }
    }
}
