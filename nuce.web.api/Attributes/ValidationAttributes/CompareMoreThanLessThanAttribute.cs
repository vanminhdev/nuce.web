using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Attributes.ValidationAttributes
{
    public class CompareMoreThanLessThanAttribute: CompareAttribute
    {
        private bool _isCompareMoreThan;

        public CompareMoreThanLessThanAttribute(bool isCompareMoreThan, string otherProperty) : base(otherProperty)
        {
            _isCompareMoreThan = isCompareMoreThan;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentProp = (DateTime)value;

            var property = validationContext.ObjectType.GetProperty(OtherProperty);

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

            if(_isCompareMoreThan)
            {
                if (currentProp < comparisonValue)
                    return new ValidationResult(ErrorMessage);
            } 
            else
            {
                if (currentProp > comparisonValue)
                    return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
