using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsEduSurveyGraduateDeThi
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NoiDungDeThi { get; set; }
        public string DapAn { get; set; }
        public int Status { get; set; }
    }
}
