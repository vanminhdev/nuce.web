using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Common
{
    public static class UserParameters
    {
        public static readonly string MSSV = "Masv";
        public static readonly string JwtAccessToken = "JWT-token";
        public static readonly string JwtRefreshToken = "JWT-refresh-token";
    }
    public static class ActivityLogParameters
    {
        public static readonly string CODE_LOGIN = "LOGIN";
        public static readonly string CODE_REGISTER = "REGISTER";
        public static readonly string CODE_CHANGE_PASSWORD = "CHANGE_PASSWORD";
        public static readonly string CODE_LOGOUT = "LOG_OUT";
    }
}
