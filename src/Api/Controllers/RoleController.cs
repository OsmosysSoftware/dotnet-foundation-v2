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
    public async Task<IActionResult> GetRoleById(int id)
    {
        RoleResponseDto? role = await _roleService.GetRoleByIdAsync(id).ConfigureAwait(false);
        if (role == null)
        {
            return NotFound(new { message = "Role not found" });
        }

        return Ok(role);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        IEnumerable<RoleResponseDto> roles = await _roleService.GetAllRolesAsync().ConfigureAwait(false);
        return Ok(roles);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto roleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        RoleResponseDto? createdRole = await _roleService.CreateRoleAsync(roleDto).ConfigureAwait(false);
        if (createdRole == null)
        {
            return BadRequest(new { message = "Failed to create role" });
        }

        return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.Id }, createdRole);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleUpdateDto roleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        bool success = await _roleService.UpdateRoleAsync(id, roleDto).ConfigureAwait(false);
        if (!success)
        {
            return NotFound(new { message = "Role not found or update failed" });
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        bool success = await _roleService.DeleteRoleAsync(id).ConfigureAwait(false);
        if (!success)
        {
            return NotFound(new { message = "Role not found or deletion failed" });
        }

        return NoContent();
    }
}