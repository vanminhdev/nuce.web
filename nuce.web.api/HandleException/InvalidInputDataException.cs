using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.HandleException
{
    public class InvalidInputDataException : Exception
    {
        public InvalidInputDataException(string message = "Trường đã nhập có kiểu dữ liệu không hợp lệ") : base(message)
        {
        }
    }
}
