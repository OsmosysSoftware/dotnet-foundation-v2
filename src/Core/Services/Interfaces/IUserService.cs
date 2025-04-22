using Core.Entities.DTOs;
using Core.Entities.Models;

namespace Core.Services.Interfaces;

public interface IUserService
{
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<UserResponseDto?> GetUserByEmailAsync(string email);
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
    Task<bool> AddUserAsync(UserCreateDto user);
    Task<bool> UpdateUserAsync(int id, UserUpdateDto user);
    Task<bool> DeleteUserAsync(int id);
}