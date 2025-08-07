using TerraMedia.Application.Dtos;

namespace TerraMedia.Application.Interfaces;

/// <summary>
/// Interface que define os serviços relacionados a livros e seus comentários.
/// </summary>
public interface IBookService
{
    /// <summary>
    /// Obtém um livro pelo seu ID junto com os comentários associados.
    /// </summary>
    /// <param name="coverId">Identificador único do livro.</param>
    /// <returns>Um DTO contendo os dados do livro e seus comentários.</returns>
    Task<BookDto> GetByIdWithCommentsAsync(int coverId);

    /// <summary>
    /// Remove (desativa) um comentário de um livro específico.
    /// </summary>
    /// <param name="bookId">Identificador único do livro.</param>
    /// <param name="commentId">Identificador único do comentário a ser removido.</param>
    /// <returns>Um DTO atualizado do livro após remoção do comentário.</returns>
    Task<BookDto> RemoveCommentAsync(Guid bookId, Guid commentId);

    /// <summary>
    /// Adiciona um novo comentário a um livro já existente.
    /// </summary>
    /// <param name="bookId">Identificador único do livro.</param>
    /// <param name="commentDto">Dados do comentário a ser adicionado.</param>
    /// <returns>Um DTO atualizado do livro com o novo comentário.</returns>
    Task<BookDto> AddCommentToExistingBookAsync(Guid bookId, Guid userId, CommentDto commentDto);

    /// <summary>
    /// Cria um novo livro e adiciona um comentário inicial a ele.
    /// </summary>
    /// <param name="coverId">Identificador do cover para criação do livro.</param>
    /// <param name="commentDto">Dados do comentário inicial.</param>
    /// <returns>Um DTO com o livro criado e seu comentário.</returns>
    Task<BookDto> CreateBookWithCommentAsync(int coverId, Guid userId, BookCommentDto commentDto);

    /// <summary>
    /// Atualiza um comentário específico de um livro.
    /// </summary>
    /// <param name="bookId">Identificador único do livro.</param>
    /// <param name="commentId">Identificador único do comentário a ser atualizado.</param>
    /// <param name="commentDto">Novos dados do comentário.</param>
    /// <returns>Um DTO atualizado do livro após a modificação do comentário.</returns>
    Task<BookDto> UpdateCommentAsync(Guid bookId, Guid userId, Guid commentId, CommentDto commentDto);
}
