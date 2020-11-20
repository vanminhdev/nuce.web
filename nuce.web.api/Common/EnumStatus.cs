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

    public enum SurveyRoundStatus
    {
        Active = 1,
        Deactive = 2,
        Deleted = 4,
    }
}
