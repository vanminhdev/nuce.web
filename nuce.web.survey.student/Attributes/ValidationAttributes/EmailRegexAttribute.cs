﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.survey.student.Attributes.ValidationAttributes
{
    /// <summary>
    /// Không chứa khoảng trắng và có dạng email
    /// </summary>
    public class EmailRegexAttribute : RegularExpressionAttribute
    {
        public EmailRegexAttribute() : base(@"^\s*(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([;.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*\s*$")
        {
        }
    }
}
