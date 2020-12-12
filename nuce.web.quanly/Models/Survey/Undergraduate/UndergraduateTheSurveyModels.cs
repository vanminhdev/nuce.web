using nuce.web.quanly.Attributes.ValidationAttributes;
using nuce.web.quanly.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.quanly.Models.Survey.Undergraduate
{
    public class UndergraduateTheSurvey
    {
        public Guid id { get; set; }
        public Guid dotKhaoSatId { get; set; }
        public Guid deThiId { get; set; }
        public string name { get; set; }
        public string noiDungDeThi { get; set; }
        public string dapAn { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime endDate { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public int type { get; set; }
        public int status { get; set; }

        public string surveyRoundName { get; set; }
    }

    public class UndergraduateTheSurveyUpdate : UndergraduateTheSurveyCreate
    {
        public string id { get; set; }
    }

    public class UndergraduateTheSurveyCreate
    {
        public Guid? dotKhaoSatId { get; set; }

        public Guid? deThiId { get; set; }

        public string name { get; set; }

        public DateTime? fromDate { get; set; }

        public DateTime? endDate { get; set; }

        public string description { get; set; }

        public string note { get; set; }

        public int? type { get; set; }
    }
}
