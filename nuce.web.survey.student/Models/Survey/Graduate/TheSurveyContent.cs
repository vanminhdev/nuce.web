using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nuce.web.survey.student.Models.Survey.Graduate
{
    public class TheSurveyStudent
    {
        public string id { get; set; }
        public string baiKhaoSatId { get; set; }
        public string name { get; set; }
        public string nganh { get; set; }
        public string chuyenNganh { get; set; }
        public int status { get; set; }
    }
}