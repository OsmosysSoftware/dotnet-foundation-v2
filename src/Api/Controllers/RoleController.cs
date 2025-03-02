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
        BaseResponse<RoleResponseDto> response = new(ResponseStatus.Fail)
        {
            Message = "Role not found"
        };
        try
        {
            RoleResponseDto? role = await _roleService.GetRoleByIdAsync(id).ConfigureAwait(false);
            if (role != null)
            {
                response.Status = ResponseStatus.Success;
                response.Data = role;
                response.Message = "Role created successfully";
            }

            return role == null ? NotFound(response) : Ok(response);
        }
        catch (Exception ex)
        {
            response.Status = ResponseStatus.Error;
            response.Message = "An unexpected error occurred.";
            response.StackTrace = ex.StackTrace;
            return StatusCode(500, response);
        }
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<IEnumerable<RoleResponseDto>>>> GetAllRoles()
    {
        BaseResponse<IEnumerable<RoleResponseDto>> response = new(ResponseStatus.Fail)
        {
            Message = "No roles found"
        };
        try
        {
            IEnumerable<RoleResponseDto> roles = await _roleService.GetAllRolesAsync().ConfigureAwait(false);
            if (roles.Any())
            {
                response.Status = ResponseStatus.Success;
                response.Data = roles;
                response.Message = "Roles retrieved successfully";
            }

            return roles.Any() ? Ok(response) : NotFound(response);
        }
        catch (Exception ex)
        {
            response.Status = ResponseStatus.Error;
            response.Message = "An unexpected error occurred.";
            response.StackTrace = ex.StackTrace;
            return StatusCode(500, response);
        }
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse<RoleResponseDto?>>> CreateRole([FromBody] RoleCreateDto roleDto)
    {
        BaseResponse<RoleResponseDto> response = new(ResponseStatus.Fail)
        {
            Message = "Failed to create role"
        };
        try
        {
            if (!ModelState.IsValid)
            {
                return ModelValidationBadRequest.GenerateErrorResponse(ModelState);
            }

            RoleResponseDto? createdRole = await _roleService.CreateRoleAsync(roleDto).ConfigureAwait(false);
            if (createdRole != null)
            {
                response.Status = ResponseStatus.Success;
                response.Data = createdRole;
                response.Message = "Role created successfully";
            }

            return createdRole == null ? BadRequest(response) : Ok(createdRole);
        }
        catch (Exception ex)
        {
            response.Status = ResponseStatus.Error;
            response.Message = "An unexpected error occurred.";
            response.StackTrace = ex.StackTrace;
            return StatusCode(500, response);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BaseResponse<RoleResponseDto>>> UpdateRole(int id, [FromBody] RoleUpdateDto roleDto)
    {
        BaseResponse<RoleResponseDto> response = new(ResponseStatus.Fail)
        {
            Message = "Role update failed"
        };

        try
        {
            if (!ModelState.IsValid)
            {
                return ModelValidationBadRequest.GenerateErrorResponse(ModelState);
            }

            RoleResponseDto? updatedRole = await _roleService.UpdateRoleAsync(id, roleDto).ConfigureAwait(false);
            if (updatedRole == null)
            {
                return NotFound(new { message = "Role not found" });
            }

            response.Status = ResponseStatus.Success;
            response.Data = updatedRole;
            response.Message = "Role updated successfully";
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Status = ResponseStatus.Error;
            response.Message = "An unexpected error occurred.";
            response.StackTrace = ex.StackTrace;
            return StatusCode(500, response);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse<bool>>> DeleteRole(int id)
    {
        BaseResponse<bool> response = new(ResponseStatus.Fail)
        {
            Message = "Role deletion failed"
        };

        try
        {
            bool isDeleted = await _roleService.DeleteRoleAsync(id).ConfigureAwait(false);
            if (!isDeleted)
            {
                response.Message = "Role not found";
                return NotFound(response);
            }

            response.Status = ResponseStatus.Success;
            response.Data = true;
            response.Message = "Role deleted successfully";
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Status = ResponseStatus.Error;
            response.Message = "An unexpected error occurred.";
            response.StackTrace = ex.StackTrace;
            return StatusCode(500, response);
        }
    }
}