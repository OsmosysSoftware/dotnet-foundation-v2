using Core.Entities.DTOs;
using Core.Entities.Models;

namespace Core.Services.Interfaces;

public interface IRoleService
{
    Task<RoleResponseDto?> GetRoleByIdAsync(int id);
    Task<int> GetTotalRolesCountAsync();
    Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync(int pageNumber, int pageSize);
    Task<RoleResponseDto?> CreateRoleAsync(RoleCreateDto roleDto);
    Task<RoleResponseDto?> UpdateRoleAsync(int id, RoleUpdateDto roleDto);
    Task<bool> DeleteRoleAsync(int id);
}
