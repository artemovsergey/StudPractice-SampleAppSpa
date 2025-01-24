using MediatR;
using SampleApp.API.Entities;
using SampleApp.API.Interfaces;

namespace SampleApp.Application.Users.Queries
{
    // public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User>
    // {
    //     private readonly IUserRepository _userRepository;
    //     public GetUserQueryHandler(IUserRepository userRepository)
    //     {
    //         _userRepository = userRepository;
    //     }
    //     public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    //     {
    //        //var user = await _userRepository.GetUser(request.UserId);
    //        return await Task.FromResult(new User());
    //     }
    // }
}