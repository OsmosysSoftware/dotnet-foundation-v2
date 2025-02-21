using Core.Entities.Models;

namespace Core.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetRoleByIdAsync(int id);
    Task<IEnumerable<Role>> GetAllRolesAsync();
    Task<Role?> AddRoleAsync(Role role);
    Task<bool> UpdateRoleAsync(Role role);
    Task<bool> DeleteRoleAsync(Role role);
}
