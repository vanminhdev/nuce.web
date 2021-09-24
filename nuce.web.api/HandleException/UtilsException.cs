using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.HandleException
{
    public class UtilsException
    {
        public static string GetMainMessage(Exception e)
        {
            var message = "";
            if (e.InnerException != null && e.InnerException.InnerException != null)
            {
                message = e.InnerException.InnerException.Message;
            }
            else if (e.InnerException != null)
            {
                message = e.InnerException.Message;
            }
            else
            {
                message = e.Message;
            }
            return message;
        }
    }
}
