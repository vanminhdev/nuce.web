using nuce.web.quanly.Attributes.ValidationAttributes;
using nuce.web.quanly.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.quanly.Models
{
    public class GraduateSurveyRound
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime endDate { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public int type { get; set; }
        public int status { get; set; }
    }

    public class GraduateSurveyRoundUpdate : GraduateSurveyRoundCreate
    {
        public string id { get; set; }
    }

    public class GraduateSurveyRoundCreate
    {
        public string name { get; set; }

        public DateTime? fromDate { get; set; }

        public DateTime? endDate { get; set; }

        public string description { get; set; }

        public string note { get; set; }

        public int? type { get; set; }
    }
}
