using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Common
{
    public enum UserStatus
    {
        Active = 1,
        Deactive = 2,
        Deleted = 4 
    }

    public enum SemesterStatus
    {
        /// <summary>
        /// Là kỳ hiện tại
        /// </summary>
        IsCurrent = 1,
        /// <summary>
        /// Là kỳ ngay trước
        /// </summary>
        IsLast = 2,
        /// <summary>
        /// Là kỳ trong quá khứ
        /// </summary>
        IsPast = 3,
        /// <summary>
        /// Bị xoá
        /// </summary>
        Deleted = 4
    }

    public enum QuestionStatus
    {
        Active = 1,
        Deleted = 4
    }

    public enum AnswerStatus
    {
        Active = 1,
        Deleted = 4
    }

    public enum ExamQuestionStatus
    {
        Active = 1,
        Deleted = 4
    }

    /// <summary>
    /// Đợt khảo sát
    /// </summary>
    public enum SurveyRoundStatus
    {
        New = 1,
        Closed = 2,
        Opened = 3,
        Deleted = 4,
        End = 5
    }

    /// <summary>
    /// Bài khảo sát
    /// </summary>
    public enum TheSurveyStatus
    {
        New = 1,
        Deactive = 2,
        Published = 3,
        Deleted = 4,
    }

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
