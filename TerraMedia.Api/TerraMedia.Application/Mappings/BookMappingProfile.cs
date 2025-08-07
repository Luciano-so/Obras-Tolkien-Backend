using AutoMapper;
using TerraMedia.Application.Dtos;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Application.Mappings;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        CreateMap<Book, BookDto>().ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));
    }
}
