using Core.Entities.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetUserByIdAsync(id).ConfigureAwait(false);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetUserByEmailAsync(email).ConfigureAwait(false);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync().ConfigureAwait(false);
    }

    public async Task<bool> AddUserAsync(User user)
    {
        return await _userRepository.AddUserAsync(user).ConfigureAwait(false);
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        return await _userRepository.UpdateUserAsync(user).ConfigureAwait(false);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        User? user = await _userRepository.GetUserByIdAsync(id).ConfigureAwait(false);
        if (user == null)
        {
            return false;
        }

        return await _userRepository.DeleteUserAsync(user).ConfigureAwait(false);
    }
}