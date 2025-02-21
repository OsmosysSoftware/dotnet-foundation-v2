using Core.Entities.DTOs;
using Core.Entities.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleResponseDto?> GetRoleByIdAsync(int id)
    {
        Role? role = await _roleRepository.GetRoleByIdAsync(id).ConfigureAwait(false);
        return role == null ? null : new RoleResponseDto { Id = role.Id, Name = role.Name };
    }

    public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync()
    {
        IEnumerable<Role> roles = await _roleRepository.GetAllRolesAsync().ConfigureAwait(false);
        return roles.Select(r => new RoleResponseDto { Id = r.Id, Name = r.Name });
    }

    public async Task<RoleResponseDto?> CreateRoleAsync(RoleCreateDto roleDto)
    {
        Role newRole = new Role { Name = roleDto.Name };

        Role? createdRole = await _roleRepository.AddRoleAsync(newRole).ConfigureAwait(false);
        return createdRole == null ? null : new RoleResponseDto { Id = createdRole.Id, Name = createdRole.Name };
    }

    public async Task<bool> UpdateRoleAsync(int id, RoleUpdateDto roleDto)
    {
        Role? existingRole = await _roleRepository.GetRoleByIdAsync(id).ConfigureAwait(false);
        if (existingRole == null)
        {
            return false;
        }

        existingRole.Name = roleDto.Name;
        return await _roleRepository.UpdateRoleAsync(existingRole).ConfigureAwait(false);
    }

    public async Task<bool> DeleteRoleAsync(int id)
    {
        Role? role = await _roleRepository.GetRoleByIdAsync(id).ConfigureAwait(false);
        if (role == null)
        {
            return false;
        }

        return await _roleRepository.DeleteRoleAsync(role).ConfigureAwait(false);
    }
}
