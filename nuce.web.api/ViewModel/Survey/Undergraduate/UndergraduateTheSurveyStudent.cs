﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey.Undergraduate
{
    public class UndergraduateTheSurveyStudent
    {
        public Guid Id { get; set; }
        public Guid BaiKhaoSatId { get; set; }
        public string Nganh { get; set; }
        public string ChuyenNganh { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
    }
}
