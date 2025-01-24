using MediatR;
using SampleApp.API.Dto;

namespace SampleApp.Application.Users.Queries
{
    public class GetUsersQuery : IRequest<List<UserRecordDto>>
    {

    }
}