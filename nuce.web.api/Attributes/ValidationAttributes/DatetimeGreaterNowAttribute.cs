using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Attributes.ValidationAttributes
{
    public class DatetimeGreaterNowAttribute: ValidationAttribute
    {
        private DateTime From { get; set; }
        private DateTime End { get; set; }

        public DatetimeGreaterNowAttribute() : base()
        {
            From = DateTime.Now;
            End = DateTime.MaxValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return ValidationResult.Success;
            }

            var val = ((DateTime)value);
            if (val >= From && val <= End)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"Date must greater than {From.ToLocalTime()} and less than {End.ToLocalTime()}");
            }
        }
    }
}
