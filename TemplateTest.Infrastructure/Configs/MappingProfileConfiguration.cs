using TemplateTest.Core.DTO.Request;
using TemplateTest.Core.DTO.Response;
using TemplateTest.Domain.Entities;
using AutoMapper;

namespace TemplateTest.Infrastructure.Configs
{
    public class MappingProfileConfiguration : Profile
    {
        public MappingProfileConfiguration()
        {
            CreateMap<User, AuthenticationResponse>(MemberList.None);
            CreateMap<User, UserResponse>(MemberList.None);
            CreateMap<BasicUser, BasicAuthResponse>(MemberList.None);
            CreateMap<AddUserRequest, User>(MemberList.None)
                .ForMember(dest => dest.DefaultRole, opt => opt.MapFrom(src => src.Role));
        }
    }
}
