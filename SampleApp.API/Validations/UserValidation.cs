using System.ComponentModel.DataAnnotations;

namespace SampleApp.API.Validations;

public class UserNameMaxLengthValidation : ValidationAttribute
{
    private readonly int _maxLength;

    public UserNameMaxLengthValidation(int maxLength) : base($"Максимальная длина имени: {maxLength} ")
    {
        _maxLength = maxLength;
    }

    public override bool IsValid(object? value)
    {
        return ((String)value!).Length <= _maxLength;
    }
}