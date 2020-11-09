using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.HandleException
{
    public class CannotCallEduWebServiceException : Exception
    {
        public CannotCallEduWebServiceException(string message) :base(message)
        {
        }
    }
}
