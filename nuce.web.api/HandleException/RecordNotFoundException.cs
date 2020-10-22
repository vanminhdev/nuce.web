using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.HandleException
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException() : base()
        {
        }

        public RecordNotFoundException(string message) : base(message)
        {
        }
    }
}
