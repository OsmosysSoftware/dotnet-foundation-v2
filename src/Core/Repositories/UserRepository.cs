using Core.DataContext;
using Core.Entities.Models;
using Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.Include(u => u.Role).AsNoTracking().FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.Include(u => u.Role).AsNoTracking().FirstOrDefaultAsync(u => u.Email == email).ConfigureAwait(false);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.Include(u => u.Role).AsNoTracking().ToListAsync().ConfigureAwait(false);
    }
    public async Task<User?> AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user).ConfigureAwait(false);
        bool success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        return success ? user : null;
    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        bool success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        return success ? user : null;
    }

    public async Task<bool> DeleteUserAsync(User user)
    {
        _context.Users.Remove(user);
        return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }
}