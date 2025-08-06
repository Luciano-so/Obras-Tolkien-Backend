using AutoMapper;
using TerraMedia.Application.Dtos;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Application.Mappings;

public class BookCommentMappingProfile : Profile
{
    public BookCommentMappingProfile()
    {
        CreateMap<BookComment, BookCommentDto>();
        CreateMap<BookCommentDto, BookComment>()
            .ConstructUsing((src, context) =>
                BookComment.Factory.Create(src.UserId, null!, src.Comment));
    }
}
