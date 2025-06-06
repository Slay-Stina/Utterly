using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Utterly.Attributes;

public class MinAgeAttribute : ValidationAttribute, IClientModelValidator
{
    private readonly int _minAge;
    public MinAgeAttribute(int minAge)
    {
        _minAge = minAge;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-minage", ErrorMessage ?? $"Du måste vara minst {_minAge} år gammal för att registrera dig.");
        MergeAttribute(context.Attributes, "data-val-minage-minage", _minAge.ToString());
    }

    private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
    {
        if (attributes.ContainsKey(key))
            return false;
        attributes.Add(key, value);
        return true;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success; // Required-attributet hanterar null

        if (!(value is DateTime birthDate))
            return new ValidationResult("Ogiltigt datumformat.");

        var today = DateTime.Today;

        if (birthDate > today)
            return new ValidationResult("Födelsedatum kan inte vara i framtiden.");

        var age = today.Year - birthDate.Year;
        if (birthDate > today.AddYears(-age)) age--;

        if (age < _minAge)
        {
            return new ValidationResult(ErrorMessage ?? $"Du måste vara minst {_minAge} år gammal för att registrera dig.");
        }
        return ValidationResult.Success;
    }
}
