﻿using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey.Graduate
{
    public class GraduateTheSurvey
    {
        public Guid Id { get; set; }
        public Guid DotKhaoSatId { get; set; }
        public Guid DeThiId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public int Status { get; set; }

        public string SurveyRoundName { get; set; }
    }

    public class GraduateTheSurveyFilter
    {
    }

    public class GraduateTheSurveyCreate : GraduateTheSurveyUpdate
    {

    }

    public class GraduateTheSurveyUpdate
    {
        [Required(AllowEmptyStrings = false)]
        public Guid? DotKhaoSatId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public Guid? DeThiId { get; set; }

        public string Description { get; set; }

        public string Note { get; set; }
    }
}
