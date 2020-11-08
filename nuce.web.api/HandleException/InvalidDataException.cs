using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.HandleException
{
    public class InvalidDataException : Exception
    {
        public InvalidDataException(string message = "Trường đã nhập có kiểu dữ liệu không hợp lệ") : base(message)
        {
        }
    }
}
