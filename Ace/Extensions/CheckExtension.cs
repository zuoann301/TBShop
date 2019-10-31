using Ace.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class CheckExtension
    {
        public static void NotNull<T>(this T val, string errorMsg = null)
        {
            if (val == null)
                throw new InvalidInputException(errorMsg);
        }
        public static void NotNullOrEmpty(this object val, string errorMsg = null)
        {
            if (val is string)
            {
                NotNullOrEmpty((string)val, errorMsg);
                return;
            }
            else
            {
                NotNull(val, errorMsg);
            }
        }
        public static void NotNullOrEmpty(this string val, string errorMsg = null)
        {
            if (string.IsNullOrEmpty(val))
                throw new InvalidInputException(errorMsg);
        }
        public static void EnsureValid(this object obj)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext vc = new ValidationContext(obj, null, null);
            bool isValid = Validator.TryValidateObject
                    (obj, vc, validationResults, true);
            if (isValid == false)
            {
                throw new InvalidInputException(validationResults[0].ErrorMessage);
            }
        }
    }
}
