using Core.Entities.Models;

namespace Core.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<int> GetTotalUsersCountAsync();
    Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber, int pageSize);
    Task<User?> AddUserAsync(User user);
    Task<User?> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(User user);
}
