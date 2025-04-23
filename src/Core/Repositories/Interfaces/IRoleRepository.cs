using Core.Entities.Models;

namespace Core.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetRoleByIdAsync(int id);
    Task<Role?> GetRoleByNameAsync(string name);
    Task<int> GetTotalRolesCountAsync();
    Task<IEnumerable<Role>> GetAllRolesAsync(int pageNumber, int pageSize);
    Task<Role?> AddRoleAsync(Role role);
    Task<Role?> UpdateRoleAsync(Role role);
    Task<bool> DeleteRoleAsync(Role role);
}
