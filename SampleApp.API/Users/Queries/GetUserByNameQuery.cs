using MediatR;
using SampleApp.API.Entities;


namespace SampleApp.Application.Users.Queries
{
    public class GetUserByNameQuery : IRequest<User>
    {
        public string UserName {get; set;} = default!;
    }
}