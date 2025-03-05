using Api.Models.Common;
using Api.Models.Enums;
using Core.Entities.DTOs;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<UserResponseDto>>> GetUserById(int id)
    {
        BaseResponse<UserResponseDto> response = new(ResponseStatus.Success);
        UserResponseDto? user = await _userService.GetUserByIdAsync(id).ConfigureAwait(false);

        response.Data = user;
        response.Message = "User retrieved successfully";
        return Ok(response);
    }

    [Authorize]
    [HttpGet("email/{email}")]
    public async Task<ActionResult<BaseResponse<UserResponseDto>>> GetUserByEmail(string email)
    {
        BaseResponse<UserResponseDto> response = new(ResponseStatus.Success);
        UserResponseDto? user = await _userService.GetUserByEmailAsync(email).ConfigureAwait(false);

        response.Data = user;
        response.Message = "User retrieved successfully";
        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<BaseResponse<IEnumerable<UserResponseDto>>>> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        BaseResponse<IEnumerable<UserResponseDto>> response = new(ResponseStatus.Success);
        IEnumerable<UserResponseDto> users = await _userService.GetAllUsersAsync(pageNumber, pageSize).ConfigureAwait(false);
        int totalCount = await _userService.GetTotalUsersCountAsync().ConfigureAwait(false);
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        response.Data = users;
        response.Message = "Users retrieved successfully";
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        string? token = await _userService.Login(loginDto.Email, loginDto.Password).ConfigureAwait(false);
        return Ok(new { token });
    }


    [HttpPost("register")]
    public async Task<ActionResult<BaseResponse<UserResponseDto>>> RegisterUser([FromBody] UserCreateDto userDto)
    {
        BaseResponse<UserResponseDto> response = new(ResponseStatus.Success);
        if (!ModelState.IsValid)
        {
            return ModelValidationBadRequest.GenerateErrorResponse(ModelState);
        }
        UserResponseDto? createdUser = await _userService.AddUserAsync(userDto).ConfigureAwait(false);

        response.Data = createdUser;
        response.Message = "User registered successfully";
        return Ok(response);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<BaseResponse<UserResponseDto>>> UpdateUser(int id, [FromBody] UserUpdateDto userDto)
    {
        BaseResponse<UserResponseDto> response = new(ResponseStatus.Success);
        if (!ModelState.IsValid)
        {
            return ModelValidationBadRequest.GenerateErrorResponse(ModelState);
        }

        UserResponseDto? updatedUser = await _userService.UpdateUserAsync(id, userDto).ConfigureAwait(false);

        response.Data = updatedUser;
        response.Message = "User updated successfully";
        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse<bool>>> DeleteUser(int id)
    {
        BaseResponse<bool> response = new(ResponseStatus.Success);
        bool isDeleted = await _userService.DeleteUserAsync(id).ConfigureAwait(false);

        response.Data = isDeleted;
        response.Message = "User deleted successfully";
        return Ok(response);
    }
}