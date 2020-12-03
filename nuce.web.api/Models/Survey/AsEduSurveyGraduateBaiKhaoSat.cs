﻿using System;
using System.Collections.Generic;

namespace nuce.web.api.Models.Survey
{
    public partial class AsEduSurveyGraduateBaiKhaoSat
    {
        public Guid Id { get; set; }
        public Guid DotKhaoSatId { get; set; }
        public Guid DeThiId { get; set; }
        public string NoiDungDeThi { get; set; }
        public string DapAn { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
    }
}
