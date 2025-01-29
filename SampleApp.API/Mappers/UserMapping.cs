using AutoMapper;
using SampleApp.API.Dto;
using SampleApp.API.Entities;

namespace SampleApp.Application.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // CreateMap<UserRecordDto, User>()
        //     .ForMember(dest => dest.PasswordHash, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Password)))
        //     .ForMember(dest => dest.PasswordSalt, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Password)));
          
        // CreateMap<User, UserRecordDto>();

        CreateMap<Micropost, MicropostDto>();
        CreateMap<User, UserRecordDto>();
    }
}
