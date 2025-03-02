using Api.Models.Common;
using Api.Models.Enums;
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
    public async Task<ActionResult<BaseResponse<UserResponseDto>>> GetUserById(int id)
    {
        BaseResponse<UserResponseDto> response = new(ResponseStatus.Fail)
        {
            Message = "User not found"
        };
        try
        {
            UserResponseDto? user = await _userService.GetUserByIdAsync(id).ConfigureAwait(false);
            if (user != null)
            {
                response.Status = ResponseStatus.Success;
                response.Data = user;
                response.Message = "User retrieved successfully";
            }
            return user == null ? NotFound(response) : Ok(response);
        }
        catch (Exception ex)
        {
            response.Status = ResponseStatus.Error;
            response.Message = "An unexpected error occurred.";
            response.StackTrace = ex.StackTrace;
            return StatusCode(500, response);
        }
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<BaseResponse<UserResponseDto>>> GetUserByEmail(string email)
    {
        BaseResponse<UserResponseDto> response = new(ResponseStatus.Fail)
        {
            Message = "User not found"
        };
        try
        {
            UserResponseDto? user = await _userService.GetUserByEmailAsync(email).ConfigureAwait(false);
            if (user != null)
            {
                response.Status = ResponseStatus.Success;
                response.Data = user;
                response.Message = "User retrieved successfully";
            }
            return user == null ? NotFound(response) : Ok(response);
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
    public async Task<ActionResult<BaseResponse<IEnumerable<UserResponseDto>>>> GetAllUsers()
    {
        BaseResponse<IEnumerable<UserResponseDto>> response = new(ResponseStatus.Fail)
        {
            Message = "No users found"
        };
        try
        {
            IEnumerable<UserResponseDto> users = await _userService.GetAllUsersAsync().ConfigureAwait(false);
            if (users.Any())
            {
                response.Status = ResponseStatus.Success;
                response.Data = users;
                response.Message = "Users retrieved successfully";
            }

            return users.Any() ? Ok(response) : NotFound(response);
        }
        catch (Exception ex)
        {
            response.Status = ResponseStatus.Error;
            response.Message = "An unexpected error occurred.";
            response.StackTrace = ex.StackTrace;
            return StatusCode(500, response);
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<BaseResponse<UserResponseDto>>> RegisterUser([FromBody] UserCreateDto userDto)
    {
        BaseResponse<UserResponseDto> response = new(ResponseStatus.Fail)
        {
            Message = "Failed to register user"
        };
        try
        {
            if (!ModelState.IsValid)
            {
                return ModelValidationBadRequest.GenerateErrorResponse(ModelState);
            }

            UserResponseDto? createdUser = await _userService.AddUserAsync(userDto).ConfigureAwait(false);
            if (createdUser != null)
            {
                response.Status = ResponseStatus.Success;
                response.Data = createdUser;
                response.Message = "User registered successfully";
            }

            return createdUser == null ? BadRequest(response) : Ok(response);
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
    public async Task<ActionResult<BaseResponse<UserResponseDto>>> UpdateUser(int id, [FromBody] UserUpdateDto userDto)
    {
        BaseResponse<UserResponseDto> response = new(ResponseStatus.Fail)
        {
            Message = "User update failed"
        };
        try
        {
            if (!ModelState.IsValid)
            {
                return ModelValidationBadRequest.GenerateErrorResponse(ModelState);
            }

            UserResponseDto? updatedUser = await _userService.UpdateUserAsync(id, userDto).ConfigureAwait(false);
            if (updatedUser == null)
            {
                return NotFound(new { message = "User not found" });
            }

            response.Status = ResponseStatus.Success;
            response.Data = updatedUser;
            response.Message = "User updated successfully";
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
    public async Task<ActionResult<BaseResponse<bool>>> DeleteUser(int id)
    {
        BaseResponse<bool> response = new(ResponseStatus.Fail)
        {
            Message = "User deletion failed"
        };
        try
        {
            bool isDeleted = await _userService.DeleteUserAsync(id).ConfigureAwait(false);
            if (!isDeleted)
            {
                response.Message = "User not found";
                return NotFound(response);
            }

            response.Status = ResponseStatus.Success;
            response.Data = true;
            response.Message = "User deleted successfully";
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