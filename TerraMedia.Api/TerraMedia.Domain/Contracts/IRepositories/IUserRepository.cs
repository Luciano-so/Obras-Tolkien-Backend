using TerraMedia.Domain.Contracts.Data;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Domain.Contracts.IRepositories;
public interface IUserRepository : IRepository
{
    void Update(User user);
    void Remove(User user);
    Task AddAsync(User user);
    Task<User?> FindAsync(Guid userId);
    Task<List<User>> GetAllAsync();
    Task<User?> Authenticate(string login, string password);
    Task<bool> ExistsUser(string login);
}
