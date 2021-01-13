using nuce.web.shared.Models.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nuce.web.survey.student.Models
{
    public class SurveyResultModel
    {
        public string FacultyCode { get; set; }
        public string DepartmentCode { get; set; }
        public string LecturerCode { get; set; }
        /// <summary>
        /// Kết quả khảo sát chi tiết
        /// </summary>
        public SurveyResultResponseModel Data { get; set; }
    }
}