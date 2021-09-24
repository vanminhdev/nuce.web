using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.survey.student.Common
{
    /// <summary>
    /// Bài khảo sát sinh viên
    /// </summary>
    public enum SurveyStudentStatus
    {
        /// <summary>
        /// Chưa được phát bài ks
        /// </summary>
        HaveNot = -1,
        /// <summary>
        /// Chưa thực hiện
        /// </summary>
        DoNot = 1,
        /// <summary>
        /// Đang thực hiện
        /// </summary>
        Doing = 2,
        /// <summary>
        /// Bị kết thúc khi đợt khảo sát kết thúc
        /// </summary>
        Close = 3,
        /// <summary>
        /// Hoàn thành
        /// </summary>
        Done = 5,
        /// <summary>
        /// Yêu cầu xác thực email số điện thoại
        /// </summary>
        RequestAuthorize = 6,
    }
}
