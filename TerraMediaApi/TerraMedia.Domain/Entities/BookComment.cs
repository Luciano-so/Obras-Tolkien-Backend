using TerraMedia.Domain.Base;
using TerraMedia.Domain.Validations;

namespace TerraMedia.Domain.Entities;

public class BookComment : Entity
{
    protected BookComment() { }

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public Guid BookId { get; private set; }
    public Book Book { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string Comment { get; private set; } = string.Empty;

    public override void Validate()
    {
        Validation.ValidateIfNull(UserId, "O campo Usuário não pode estar vazio.");
        Validation.ValidateIfNull(BookId, "O campo Livro não pode estar vazio.");

        Validation.ValidateIfFalse(CreatedAt > DateTime.MinValue, "A Data de cadastro deve ser válida.");
        Validation.ValidateIfFalse(UpdatedAt > DateTime.MinValue, "A Data de atualização deve ser válida.");

        Validation.ValidateIfEmpty(Comment, "O campo Comentário não pode estar vazio.");
        Validation.ValidateLength(Comment, 5, 1000, "O comentário deve conter entre {0} e {1} caracteres.");
    }

    public void UpdateComment(string novoComentario)
    {
        Comment = novoComentario.Trim();
        UpdatedAt = GenerateDate();
        Validate();
    }

    private static DateTime GenerateDate() => DateTime.UtcNow;

    public static class Factory
    {
        public static BookComment Create(Guid userId, Book book, string comment)
        {
            var date = GenerateDate();

            var info = new BookComment
            {
                UserId = userId,
                Book = book,
                BookId = book.Id,
                Comment = comment,
                CreatedAt = date,
                UpdatedAt = date
            };

            info.Validate();
            return info;
        }
    }
}