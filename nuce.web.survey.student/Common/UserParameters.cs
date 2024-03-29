﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.survey.student.Common
{
    public static class UserParameters
    {
        public static string UserCode = "UserCode";
        public static string JwtAccessToken = "JWT-token";
        public static string JwtRefreshToken = "JWT-refresh-token";
        public static string Roles = "Roles";
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
