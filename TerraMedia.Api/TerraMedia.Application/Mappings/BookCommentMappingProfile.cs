using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using TerraMedia.Application.Dtos;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Application.Mappings;
[ExcludeFromCodeCoverage]
public class BookCommentMappingProfile : Profile
{
    public BookCommentMappingProfile()
    {
        CreateMap<BookCommentDto, BookComment>()
            .ConstructUsing((src, context) =>
                BookComment.Factory.Create(src.UserId, null!, src.Comment));

        CreateMap<BookComment, BookCommentDto>()
        .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.Name));
    }
}
