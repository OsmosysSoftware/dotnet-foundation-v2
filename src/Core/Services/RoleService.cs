using AutoMapper;
using Core.Entities.DTOs;
using Core.Entities.Models;
using Core.Exceptions;
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
        if (id <= 0)
        {
            throw new BadRequestException("Id should be greater than 0.");
        }
        Role? role = await _roleRepository.GetRoleByIdAsync(id).ConfigureAwait(false);
        if (role == null)
        {
            throw new NotFoundException($"Role with ID {id} not found.");
        }
        return _mapper.Map<RoleResponseDto>(role);
    }

    public async Task<int> GetTotalRolesCountAsync()
    {
        return await _roleRepository.GetTotalRolesCountAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0 || pageSize <= 0)
        {
            throw new BadRequestException("pageNumber and pageSize must be greater than zero.");
        }
        IEnumerable<Role> roles = await _roleRepository.GetAllRolesAsync(pageNumber, pageSize).ConfigureAwait(false);
        if (!roles.Any())
        {
            throw new NotFoundException("No roles found.");
        }
        return _mapper.Map<IEnumerable<RoleResponseDto>>(roles);
    }

    public async Task<RoleResponseDto?> CreateRoleAsync(RoleCreateDto roleDto)
    {
        Role? existingRole = await _roleRepository.GetRoleByNameAsync(roleDto.Name).ConfigureAwait(false);
        if (existingRole != null)
        {
            throw new AlreadyExistsException($"Role with name {roleDto.Name} already exists.");
        }
        Role newRole = _mapper.Map<Role>(roleDto);

        Role? createdRole = await _roleRepository.AddRoleAsync(newRole).ConfigureAwait(false);
        if (createdRole == null)
        {
            throw new DatabaseOperationException("Failed to create role.");
        }
        return _mapper.Map<RoleResponseDto>(createdRole);
    }

    public async Task<RoleResponseDto?> UpdateRoleAsync(int id, RoleUpdateDto roleDto)
    {
        Role? existingRole = await _roleRepository.GetRoleByIdAsync(id).ConfigureAwait(false);
        if (existingRole == null)
        {
            throw new NotFoundException($"Role with ID {id} not found.");
        }

        Role? roleWithSameName = await _roleRepository.GetRoleByNameAsync(roleDto.Name).ConfigureAwait(false);
        if (roleWithSameName != null && roleWithSameName.Id != id)
        {
            throw new AlreadyExistsException($"Role with name {roleDto.Name} already exists.");
        }

        _mapper.Map(roleDto, existingRole);
        Role? updatedRole = await _roleRepository.UpdateRoleAsync(existingRole).ConfigureAwait(false);
        if (updatedRole == null)
        {
            throw new DatabaseOperationException("Failed to update role.");
        }
        return _mapper.Map<RoleResponseDto>(updatedRole);
    }

    public async Task<bool> DeleteRoleAsync(int id)
    {
        Role? role = await _roleRepository.GetRoleByIdAsync(id).ConfigureAwait(false);
        if (role == null)
        {
            throw new NotFoundException($"Role with ID {id} not found.");
        }

        bool success = await _roleRepository.DeleteRoleAsync(role).ConfigureAwait(false);
        if (!success)
        {
            throw new DatabaseOperationException("Failed to delete role.");
        }
        return success;
    }
}
