using FluentValidation;
using SampleApp.API.Entities;

namespace SampleApp.API.Validations;

public class FluentValidator : AbstractValidator<User>
    {
        public FluentValidator()
        {
            RuleFor(u => u.Login).Must(StartsWithCapitalLetter).WithMessage("Login пользователя должнен начинаться с заглавной буквы");
        }
        
        private bool StartsWithCapitalLetter(string username)
        {
            return char.IsUpper(username[0]);
        }
    }