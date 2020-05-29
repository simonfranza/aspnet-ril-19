using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TestGenerator.Model.Entities;

namespace TestGenerator.Model.Helpers
{
    public class ExamDateValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dt = (DateTime)value;

            return dt >= DateTime.UtcNow
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage ?? "La date de clôture doit être supérieure à la date du jour.");
        }

    }
}
