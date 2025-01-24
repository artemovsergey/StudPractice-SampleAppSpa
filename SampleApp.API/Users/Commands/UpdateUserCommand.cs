using MediatR;
using SampleApp.API.Entities;


namespace SampleApp.Application.Users.Commands
{
    public class UpdateUserCommand : IRequest<User>
    {
        public User? User {get;set;}
        public int userId {get ;set;}
    }
}