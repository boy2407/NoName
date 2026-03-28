using AutoMapper;
using NoName.Application.Features.Users.Commands.RegisterUser;
using NoName.Domain.Entities;

namespace NoName.Application.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // Config map từ RegisterUser sang User
            CreateMap<RegisterUserCommand, User>();
        }
    }
}