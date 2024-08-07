using Tick.Core.DTO.Request;
using Tick.Core.DTO.Response;
using Tick.Domain.Entities;
using AutoMapper;

namespace Tick.Infrastructure.Configs
{
    public class MappingProfileConfiguration : Profile
    {
        public MappingProfileConfiguration()
        {
            CreateMap<Ticker, AuthenticationResponse>(MemberList.None);
            CreateMap<Ticker, UserResponse>(MemberList.None);
            CreateMap<BasicUser, BasicAuthResponse>(MemberList.None);
            CreateMap<AddUserRequest, Ticker>(MemberList.None)
                .ForMember(dest => dest.DefaultRole, opt => opt.MapFrom(src => src.Role));
        }
    }
}
