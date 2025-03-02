using Core.DataContext;
using Core.Entities.Models;
using Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly DatabaseContext _context;

    public RoleRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetRoleByIdAsync(int id)
    {
        return await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id).ConfigureAwait(false);
    }

    public async Task<Role?> GetRoleByNameAsync(string name)
    {
        return await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower()).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        return await _context.Roles.AsNoTracking().ToListAsync().ConfigureAwait(false);
    }

    public async Task<Role?> AddRoleAsync(Role role)
    {
        await _context.Roles.AddAsync(role).ConfigureAwait(false);
        bool success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        return success ? role : null;
    }

    public async Task<Role?> UpdateRoleAsync(Role role)
    {
        _context.Roles.Update(role);
        bool success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        return success ? role : null;
    }

    public async Task<bool> DeleteRoleAsync(Role role)
    {
        _context.Roles.Remove(role);
        return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }
}
