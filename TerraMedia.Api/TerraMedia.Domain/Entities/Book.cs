using TerraMedia.Domain.Base;
using TerraMedia.Domain.Validations;

namespace TerraMedia.Domain.Entities;

public class Book : Entity
{
    protected Book()
    {
        Comments = [];
    }

    public int CoverId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public ICollection<BookComment> Comments { get; private set; }

    public void AddComment(BookComment comment)
    {
        comment.Validate();
        Comments.Add(comment);
    }

    public bool RemoveComment(Guid commentId)
    {
        var comment = Comments.First(c => c.Id == commentId);
        Validation.ValidateIfNull(comment, "Comentário não encontrado.");

        return Comments.Remove(comment);
    }

    public override void Validate()
    {
        Validation.ValidateMinMax(CoverId, 1, int.MaxValue, "O código do livro deve ser maior que zero.");
    }

    public static class Factory
    {
        public static Book Create(int coverId)
        {
            var book = new Book
            {
                CoverId = coverId,
                CreatedAt = DateTime.UtcNow
            };

            book.Validate();

            return book;
        }
    }
}
