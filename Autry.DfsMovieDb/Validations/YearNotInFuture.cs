using System;
using System.ComponentModel.DataAnnotations;

namespace Autry.DfsMovieDb.Validations
{
    public class YearNotInFuture : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            if ((int)value > DateTime.UtcNow.Year)
            {
                return new ValidationResult(ErrorMessage ?? "Movie year cannot be greater than the current year.");
            }

            return ValidationResult.Success;
        }
    }
}
