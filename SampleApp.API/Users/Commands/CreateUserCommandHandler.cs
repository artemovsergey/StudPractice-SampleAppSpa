using MediatR;
using SampleApp.API.Entities;
using SampleApp.API.Interfaces;


namespace SampleApp.Application.Users.Commands
{
    // public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand,User>
    // {
    //     private readonly IUserRepository _userRepository;
    //     private readonly IUnitOfWork _unitOfWork;
    //     public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    //     {
    //        _userRepository = userRepository;
    //        _unitOfWork = unitOfWork;   
    //     }

    //     public async Task<User> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    //     {
    //         // var validator = new CreateUserCommandValidator();
    //         // var validatorResult = await validator.ValidateAsync(command);

    //         // if(validatorResult.Errors.Count > 0){
    //         //     throw new Exceptions.ValidationException(validatorResult);
    //         // }

    //         //var response = await _userRepository.CreateUserAsync(command.User!);

    //         return await Task.FromResult(new User());
    //     }
    // }
}