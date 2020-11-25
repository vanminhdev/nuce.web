using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.HandleException
{
    public class TableBusyException : Exception
    {
        public TableBusyException(string message) : base(message)
        {
        }
    }
}
