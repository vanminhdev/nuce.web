using System;
using System.Collections.Generic;
using System.Text;

namespace nuce.web.shared.Common
{
    public static class UserParameters
    {
        public static readonly string UserCode = "UserCode";
        public static readonly string FullName = "FullName";
        public static readonly string JwtAccessToken = "JWT-token";
        public static readonly string JwtRefreshToken = "JWT-refresh-token";
        public static readonly List<LoginType> LoginViaDaotao = new List<LoginType> { LoginType.Student, LoginType.Lecturer };
        /// <summary>
        /// Loại người dùng đăng nhập
        /// </summary>
        public static readonly string UserType = "UserType";
    }

    public enum LoginType
    {
        Common = 0,
        Student = 1,
        Faculty = 2,
        Department = 3,
        Lecturer = 4
    };
}
