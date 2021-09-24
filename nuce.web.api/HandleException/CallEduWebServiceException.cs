using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.HandleException
{
    public class CallEduWebServiceException : Exception
    {
        public CallEduWebServiceException(string message) :base(message)
        {
        }
    }
}
