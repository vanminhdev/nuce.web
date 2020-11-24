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

    /// <summary>
    /// Đợt khảo sát
    /// </summary>
    public enum SurveyRoundStatus
    {
        Active = 1,
        Deactive = 2,
        Deleted = 4,
    }

    /// <summary>
    /// Bài khảo sát
    /// </summary>
    public enum TheSurveyStatus
    {
        Active = 1,
        Deactive = 2,
        Deleted = 4,
    }

    /// <summary>
    /// Bài khảo sát sinh viên
    /// </summary>
    public enum SurveyStudentStatus
    {
        DoNot = 1,
        Doing = 2,
        Done = 5
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
