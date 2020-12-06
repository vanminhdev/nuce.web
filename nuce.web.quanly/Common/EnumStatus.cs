using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.quanly.Common
{
    public enum UserStatus
    {
        Active = 1,
        Deactive = 2,
        Deleted = 4
    }

    public enum QuestionStatus
    {
        Active = 1,
        Deactive = 4
    }

    public enum AnswerStatus
    {
        Active = 1,
        Deactive = 4
    }

    /// <summary>
    /// Trạng thái làm việc trên các bảng mất nhiều thời gian
    /// </summary>
    public enum TableTaskStatus
    {
        /// <summary>
        /// chưa làm
        /// </summary>
        DoNot = 1,
        /// <summary>
        /// Đang làm
        /// </summary>
        Doing = 2,
        /// <summary>
        /// Hoàn thành
        /// </summary>
        Done = 5
    }
}
