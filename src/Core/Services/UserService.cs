using Core.Entities.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Core.Entities.DTOs;

namespace Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        User? user = await _userRepository.GetUserByIdAsync(id).ConfigureAwait(false);
        return user == null ? null : MapToUserResponseDto(user);
    }

    public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
    {
        User? user = await _userRepository.GetUserByEmailAsync(email).ConfigureAwait(false);
        return user == null ? null : MapToUserResponseDto(user);
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
        IEnumerable<User> users = await _userRepository.GetAllUsersAsync().ConfigureAwait(false);
        return users.Select(MapToUserResponseDto);
    }

    public async Task<bool> AddUserAsync(UserCreateDto userDto)
    {
        User user = new User
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            PasswordHash = HashPassword(userDto.Password),
            RoleId = userDto.RoleId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _userRepository.AddUserAsync(user).ConfigureAwait(false);
    }

    public async Task<bool> UpdateUserAsync(int id, UserUpdateDto userDto)
    {
        User? user = await _userRepository.GetUserByIdAsync(id).ConfigureAwait(false);
        if (user == null)
        {
            return false;
        }

        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.RoleId = userDto.RoleId;
        user.UpdatedAt = DateTime.UtcNow;

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

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static UserResponseDto MapToUserResponseDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role?.Name ?? "Unknown"
        };
    }
}
