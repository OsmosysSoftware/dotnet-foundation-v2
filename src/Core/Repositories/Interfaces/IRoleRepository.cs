using Core.Entities.Models;

namespace Core.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetRoleByIdAsync(int id);
    Task<Role?> GetRoleByNameAsync(string name);
    Task<IEnumerable<Role>> GetAllRolesAsync();
    Task<Role?> AddRoleAsync(Role role);
    Task<Role?> UpdateRoleAsync(Role role);
    Task<bool> DeleteRoleAsync(Role role);
}
