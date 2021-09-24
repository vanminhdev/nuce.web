using nuce.web.quanly.Attributes.ValidationAttributes;
using nuce.web.quanly.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.quanly.Models.Survey.Undergraduate
{
    public class UndergraduateSurveyRound
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? endDate { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public int status { get; set; }
    }

    public class UndergraduateSurveyRoundUpdate : UndergraduateSurveyRoundCreate
    {
        public string id { get; set; }
    }

    public class UndergraduateSurveyRoundCreate
    {
        public string name { get; set; }

        public DateTime? fromDate { get; set; }

        public DateTime? endDate { get; set; }

        public string description { get; set; }

        public string note { get; set; }
    }
}
