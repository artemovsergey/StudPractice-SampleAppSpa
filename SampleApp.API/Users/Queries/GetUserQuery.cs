using MediatR;
using SampleApp.API.Entities;

namespace SampleApp.Application.Users.Queries
{
    public class GetUserQuery : IRequest<User>
    {
        public int UserId {get;set;}
    }
}