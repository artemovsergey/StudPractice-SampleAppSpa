using FluentValidation;
using MediatR;
using SampleApp.API.Entities;

namespace SampleApp.Application.Users.Commands;

public class CreateUserCommand : IRequest<User>
{
    public User? User {get ;set;}
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>{

    public CreateUserCommandValidator()
    {
        RuleFor(u => u.User!.Name).NotEmpty().WithMessage("У пользователя должно быть имя");
    }
}
