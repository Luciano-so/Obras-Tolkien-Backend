using AutoMapper;
using TerraMedia.Application.Dtos;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateUserDto, User>()
            .ConstructUsing(dto => User.Factory.Create(dto.Name, dto.Login, dto.Password));

        CreateMap<UpdatePasswordDto, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
        
        CreateMap<User, UserDto>();

    }
}
