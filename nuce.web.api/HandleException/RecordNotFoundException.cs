using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.HandleException
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException() : base(message: "Không tìm thấy bản ghi")
        {
        }

        public RecordNotFoundException(string message = "Không tìm thấy bản ghi") : base(message)
        {
            
        }
    }
}
