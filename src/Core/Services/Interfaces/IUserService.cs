using Core.Entities.DTOs;
using Core.Entities.Models;

namespace Core.Services.Interfaces;

public interface IUserService
{
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<UserResponseDto?> GetUserByEmailAsync(string email);
    Task<int> GetTotalUsersCountAsync();
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync(int pageNumber, int pageSize);
    Task<UserResponseDto?> AddUserAsync(UserCreateDto user);
    Task<UserResponseDto?> UpdateUserAsync(int id, UserUpdateDto user);
    Task<bool> DeleteUserAsync(int id);
}