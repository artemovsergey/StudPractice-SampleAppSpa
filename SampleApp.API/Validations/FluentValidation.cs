using SampleApp.API.Entities;
using FluentValidation;

namespace SampleApp.API.Validations;

public class FluentValidator : AbstractValidator<User>
{
    public FluentValidator()
    {
        RuleFor(u => u.Name).Must(StartsWithCapitalLetter).WithMessage("Имя пользователя должно начинаться с заглавной буквы");
    }
    
    private bool StartsWithCapitalLetter(string username)
    {
        return char.IsUpper(username[0]);
    }
}