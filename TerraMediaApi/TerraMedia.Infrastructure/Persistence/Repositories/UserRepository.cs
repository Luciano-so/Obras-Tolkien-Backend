using Microsoft.EntityFrameworkCore;
using TerraMedia.Domain.Contracts.Data;
using TerraMedia.Domain.Contracts.IRepositories;
using TerraMedia.Domain.Entities;
using TerraMedia.Infrastructure.Persistence.Contexts;

namespace TerraMedia.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MainContext _context;

    public IUnitOfWork UnitOfWork => _context;
    public UserRepository(MainContext context) => _context = context;

    public async Task AddAsync(User user) => await _context.Users.AddAsync(user);

    public void Update(User user) => _context.Users.Update(user);
    public void Remove(User user) => _context.Users.Remove(user);

    public async Task<User?> Authenticate(string login, string password)
    {
        return await _context.Users.FirstOrDefaultAsync(t => t.Login.Trim().ToUpper() == login.Trim().ToUpper() && t.Password == password);
    }
}
