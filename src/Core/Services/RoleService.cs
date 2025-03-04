using AutoMapper;
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
    private readonly IMapper _mapper;

    public RoleService(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<RoleResponseDto?> GetRoleByIdAsync(int id)
    {
        Role? role = await _roleRepository.GetRoleByIdAsync(id).ConfigureAwait(false);
        return role == null ? null : _mapper.Map<RoleResponseDto>(role);
    }

    public async Task<int> GetTotalRolesCountAsync()
    {
        return await _roleRepository.GetTotalRolesCountAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync(int pageNumber, int pageSize)
    {
        IEnumerable<Role> roles = await _roleRepository.GetAllRolesAsync(pageNumber, pageSize).ConfigureAwait(false);
        return _mapper.Map<IEnumerable<RoleResponseDto>>(roles);
    }

    public async Task<RoleResponseDto?> CreateRoleAsync(RoleCreateDto roleDto)
    {
        Role? existingRole = await _roleRepository.GetRoleByNameAsync(roleDto.Name).ConfigureAwait(false);
        if (existingRole != null)
        {
            return null;
        }
        Role newRole = _mapper.Map<Role>(roleDto);

        Role? createdRole = await _roleRepository.AddRoleAsync(newRole).ConfigureAwait(false);
        return createdRole == null ? null : _mapper.Map<RoleResponseDto>(createdRole);
    }

    public async Task<RoleResponseDto?> UpdateRoleAsync(int id, RoleUpdateDto roleDto)
    {
        Role? existingRole = await _roleRepository.GetRoleByIdAsync(id).ConfigureAwait(false);
        if (existingRole == null)
        {
            return null;
        }

        Role? roleWithSameName = await _roleRepository.GetRoleByNameAsync(roleDto.Name).ConfigureAwait(false);
        if (roleWithSameName != null && roleWithSameName.Id != id)
        {
            return null;
        }

        _mapper.Map(roleDto, existingRole);
        Role? updatedRole = await _roleRepository.UpdateRoleAsync(existingRole).ConfigureAwait(false);
        return updatedRole == null ? null : _mapper.Map<RoleResponseDto>(updatedRole);
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
