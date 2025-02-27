using Core.Entities.DTOs;
using Core.Entities.Models;

namespace Core.Services.Interfaces;

public interface IRoleService
{
    Task<RoleResponseDto?> GetRoleByIdAsync(int id);
    Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync();
    Task<RoleResponseDto?> CreateRoleAsync(RoleCreateDto roleDto);
    Task<bool> UpdateRoleAsync(int id, RoleUpdateDto roleDto);
    Task<bool> DeleteRoleAsync(int id);
}
