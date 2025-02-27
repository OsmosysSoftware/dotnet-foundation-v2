using Core.Entities.DTOs;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        UserResponseDto? user = await _userService.GetUserByIdAsync(id).ConfigureAwait(false);
        return user == null ? NotFound(new { message = "User not found" }) : Ok(user);
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        UserResponseDto? user = await _userService.GetUserByEmailAsync(email).ConfigureAwait(false);
        return user == null ? NotFound(new { message = "User not found" }) : Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        IEnumerable<UserResponseDto> users = await _userService.GetAllUsersAsync().ConfigureAwait(false);
        return Ok(users);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserCreateDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        bool success = await _userService.AddUserAsync(userDto).ConfigureAwait(false);
        return success ? StatusCode(201, new { message = "User registered successfully" }) : BadRequest(new { message = "Failed to register user" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userDto)
    {
        bool success = await _userService.UpdateUserAsync(id, userDto).ConfigureAwait(false);
        return success ? NoContent() : NotFound(new { message = "User not found or update failed" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        bool success = await _userService.DeleteUserAsync(id).ConfigureAwait(false);
        return success ? NoContent() : NotFound(new { message = "User not found or deletion failed" });
    }
}