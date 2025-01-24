using MediatR;
using SampleApp.API.Entities;
using SampleApp.API.Interfaces;

namespace SampleApp.Application.Users.Commands
{
    // public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, User>
    // {

    //     private readonly IUserRepository _userRepository;
    //     public DeleteUserCommandHandler(IUserRepository userRepository)
    //     {
    //         _userRepository = userRepository;
    //     }

    //     public async Task<User> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    //     {
    //         return  await Task.FromResult(new User()); //await _userRepository.DeleteUserAsync(command.userId);
    //     }
    // }
}