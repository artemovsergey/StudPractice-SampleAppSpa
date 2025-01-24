using MediatR;
using SampleApp.API.Entities;
using SampleApp.API.Interfaces;


namespace SampleApp.Application.Users.Queries
{
    // public class GetUserByNameQueryHandler : IRequestHandler<GetUserByNameQuery, User>
    // {
    //     private readonly IUserRepository _userRepository;
    //     public GetUserByNameQueryHandler(IUserRepository userRepository)
    //     {
    //         _userRepository = userRepository;
    //     }

    //     public async Task<User> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
    //     {
    //        //var user = await _userRepository.GetUserByName(request.UserName);

    //        return await Task.FromResult(new User());
    //     }
    // }
}