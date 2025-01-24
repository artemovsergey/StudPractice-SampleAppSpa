using AutoMapper;
using MediatR;
using SampleApp.API.Dto;
using SampleApp.API.Interfaces;

namespace SampleApp.Application.Users.Queries
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserRecordDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserRecordDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            //IEnumerable<User> users = await _userRepository.GetUsers();
            //var usersDto = _mapper.Map<List<UserDto>>(users);
        
            return  await Task.FromResult(new List<UserRecordDto>());
        }
    }
}