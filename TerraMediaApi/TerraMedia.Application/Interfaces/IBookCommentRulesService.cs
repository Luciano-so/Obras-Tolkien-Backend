using TerraMedia.Domain.Entities;

namespace TerraMedia.Application.Interfaces;

public interface IBookCommentRulesService
{
    Task IsAllowedToCommentAsync(Guid userId, Book book);
}
