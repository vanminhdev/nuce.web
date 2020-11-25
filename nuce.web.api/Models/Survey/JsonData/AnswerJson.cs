﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Models.Survey.JsonData
{
    public class AnswerJson
    {
        public string Code { get; set; }
        public string Content { get; set; }
    }

    public class SelectedAnswer
    {
        public string QuestionCode { get; set; }
        public string AnswerCode { get; set; }
        public List<string> AnswerCodes { get; set; }
        public string AnswerContent { get; set; }
    }
}
