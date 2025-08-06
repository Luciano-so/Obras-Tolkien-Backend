using System;
using System.Threading.Tasks;
using TerraMedia.Domain.Contracts.Data;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Domain.Contracts.IRepositories;

/// <summary>
/// Interface para o repositório de livros, estendendo a interface genérica IRepository.
/// Define métodos específicos para manipulação de livros e seus comentários.
/// </summary>
public interface IBookRepository : IRepository
{
    /// <summary>
    /// Adiciona um novo livro ao contexto.
    /// </summary>
    /// <param name="book">O objeto livro a ser adicionado.</param>
    /// <returns>Tarefa assíncrona.</returns>
    Task AddAsync(Book book);

    /// <summary>
    /// Obtém um livro pelo código do cover.
    /// </summary>
    /// <param name="coverId">Identificador do cover.</param>
    /// <returns>O livro correspondente ao coverId.</returns>
    Task<Book?> GetBook(int coverId);

    /// <summary>
    /// Marca um comentário como adicionado no contexto para que seja persistido.
    /// </summary>
    /// <param name="comment">O comentário a ser marcado como adicionado.</param>
    /// <returns>Tarefa assíncrona.</returns>
    Task MarkCommentAsAddedAsync(BookComment comment);

    /// <summary>
    /// Obtém um livro pelo seu identificador único (ID) incluindo seus comentários.
    /// </summary>
    /// <param name="bookId">Identificador único do livro.</param>
    /// <returns>O livro com seus comentários, ou null se não encontrado.</returns>
    Task<Book?> GetByIdWithCommentsAsync(Guid bookId);
}
