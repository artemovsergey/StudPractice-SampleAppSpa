using MediatR;
using SampleApp.API.Entities;

namespace SampleApp.Application.Users.Commands
{
    public class DeleteUserCommand : IRequest<User>
    {
        public int userId { get; set;}
    }
}