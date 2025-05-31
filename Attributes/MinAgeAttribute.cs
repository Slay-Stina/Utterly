using System.ComponentModel.DataAnnotations;

namespace Utterly.Attributes;

public class MinAgeAttribute : ValidationAttribute
{
    private readonly int _minAge;
    public MinAgeAttribute(int minAge)
    {
        _minAge = minAge;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;

            if (age < _minAge)
            {
                return new ValidationResult(ErrorMessage ?? $"Du m�ste vara minst {_minAge} �r gammal f�r att registrera dig.");
            }
        }
        return ValidationResult.Success;
    }
}
