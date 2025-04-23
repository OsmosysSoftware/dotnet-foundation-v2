using Api.Models.Common;
using Api.Models.Enums;
using Core.Entities.DTOs;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<RoleResponseDto>>> GetRoleById(int id)
    {
        BaseResponse<RoleResponseDto> response = new(ResponseStatus.Success);
        RoleResponseDto? role = await _roleService.GetRoleByIdAsync(id).ConfigureAwait(false);
        response.Data = role;
        response.Message = "Role retrieved successfully";
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<IEnumerable<RoleResponseDto>>>> GetAllRoles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        BaseResponse<IEnumerable<RoleResponseDto>> response = new(ResponseStatus.Success);
        IEnumerable<RoleResponseDto> roles = await _roleService.GetAllRolesAsync(pageNumber, pageSize).ConfigureAwait(false);
        int totalCount = await _roleService.GetTotalRolesCountAsync().ConfigureAwait(false);
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        response.Data = roles;
        response.Message = "Roles retrieved successfully";
        response.Pagination = new PaginationMetadata
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < totalPages
        };
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse<RoleResponseDto?>>> CreateRole([FromBody] RoleCreateDto roleDto)
    {
        BaseResponse<RoleResponseDto> response = new(ResponseStatus.Success);
        if (!ModelState.IsValid)
        {
            return ModelValidationBadRequest.GenerateErrorResponse(ModelState);
        }

        RoleResponseDto? createdRole = await _roleService.CreateRoleAsync(roleDto).ConfigureAwait(false);
        response.Data = createdRole;
        response.Message = "Role created successfully";
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BaseResponse<RoleResponseDto>>> UpdateRole(int id, [FromBody] RoleUpdateDto roleDto)
    {
        BaseResponse<RoleResponseDto> response = new(ResponseStatus.Success);
        if (!ModelState.IsValid)
        {
            return ModelValidationBadRequest.GenerateErrorResponse(ModelState);
        }

        RoleResponseDto? updatedRole = await _roleService.UpdateRoleAsync(id, roleDto).ConfigureAwait(false);

        response.Data = updatedRole;
        response.Message = "Role updated successfully";
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse<bool>>> DeleteRole(int id)
    {
        BaseResponse<bool> response = new(ResponseStatus.Success);
        bool isDeleted = await _roleService.DeleteRoleAsync(id).ConfigureAwait(false);

        response.Data = isDeleted;
        response.Message = "Role deactivated successfully";
        return Ok(response);
    }
}