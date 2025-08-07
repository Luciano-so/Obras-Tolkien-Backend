using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using TerraMedia.Application.Dtos;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Application.Mappings;
public class BookMappingProfile : Profile
{
    [ExcludeFromCodeCoverage]
    public BookMappingProfile()
    {
        CreateMap<Book, BookDto>().ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));
    }
}
