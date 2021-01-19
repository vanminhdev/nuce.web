using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Common
{
    public static class UserParameters
    {
        public static readonly string UserCode = "UserCode";
        public static readonly string FullName = "FullName";
        public static readonly string JwtAccessToken = "JWT-token";
        public static readonly string JwtRefreshToken = "JWT-refresh-token";
        public static readonly List<LoginUserType> LoginViaDaotao = new List<LoginUserType> { LoginUserType.Student, LoginUserType.Lecturer };
        /// <summary>
        /// Loại người dùng đăng nhập
        /// </summary>
        public static readonly string UserType = "UserType";
    }
    public static class ActivityLogParameters
    {
        public static readonly string CODE_LOGIN = "LOGIN";
        public static readonly string CODE_LOGIN_STUDENT_EDU_EMAIL = "LOGIN_STUDENT_EDU_EMAIL";
        public static readonly string CODE_REGISTER = "REGISTER";
        public static readonly string CODE_CHANGE_PASSWORD = "CHANGE_PASSWORD";
        public static readonly string CODE_LOGOUT = "LOG_OUT";
        public static readonly string CODE_BACKUP_DATABASE = "CODE_BACKUP_DATABASE";
        public static readonly string CODE_RESTORE_DATABASE = "CODE_RESTORE_DATABASE";
        public static readonly string CODE_DOWNLOAD_FILE_BACKUP = "CODE_DOWNLOAD_FILE_BACKUP";
        public static readonly string CODE_DELETE_FILE_BACKUP = "CODE_DELETE_FILE_BACKUP";
    }

    public enum LoginUserType
    {
        Common = 0,
        Student = 1,
        Faculty = 2,
        Department = 3,
        Lecturer = 4
    };

}
