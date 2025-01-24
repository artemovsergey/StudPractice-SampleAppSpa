using MediatR;
using SampleApp.API.Entities;
using SampleApp.API.Interfaces;

namespace SampleApp.Application.Users.Commands
{
    // public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, User>
    // {
    //     private readonly IUserRepository _userRepository;
    //     private readonly IUnitOfWork _unitOfWork;
    //     public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    //     {
    //        _userRepository = userRepository;
    //        _unitOfWork = unitOfWork;

    //     }

    //     public async Task<User> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    //     {
    //         //var user = await _userRepository.UpdateUser(command.User!,command.userId);

    //         return await Task.FromResult(new User());
    //     }
    // }
}