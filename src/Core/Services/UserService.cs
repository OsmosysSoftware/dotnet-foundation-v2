using Core.Entities.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Core.Entities.DTOs;
using AutoMapper;
using Core.Exceptions;

namespace Core.Services;

public class UserService : IUserService
{
    private readonly ITokenService _tokenService;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(ITokenService tokenService, IRoleRepository roleRepository, IUserRepository userRepository, IMapper mapper)
    {
        _tokenService = tokenService;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<string?> Login(string email, string password)
    {
        User? user = await _userRepository.GetUserByEmailAsync(email).ConfigureAwait(false);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        return _tokenService.GenerateToken(user);
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new BadRequestException("Id should be greater than 0.");
        }
        User? user = await _userRepository.GetUserByIdAsync(id).ConfigureAwait(false);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {id} not found.");
        }

        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
    {
        User? user = await _userRepository.GetUserByEmailAsync(email).ConfigureAwait(false);
        if (user == null)
        {
            throw new NotFoundException($"User with email {email} not found.");
        }

        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<int> GetTotalUsersCountAsync()
    {
        return await _userRepository.GetTotalUsersCountAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync(int pageNumber, int pageSize)
    {
        IEnumerable<User> users = await _userRepository.GetAllUsersAsync(pageNumber, pageSize).ConfigureAwait(false);
        if (!users.Any())
        {
            throw new NotFoundException("No users found.");
        }
        return _mapper.Map<IEnumerable<UserResponseDto>>(users);
    }

    public async Task<UserResponseDto?> AddUserAsync(UserCreateDto userDto)
    {
        User? existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email).ConfigureAwait(false);
        if (existingUser != null)
        {
            throw new AlreadyExistsException($"User with email {userDto.Email} already exists.");
        }

        Role? role = await _roleRepository.GetRoleByIdAsync(userDto.RoleId).ConfigureAwait(false);
        if (role == null)
        {
            throw new NotFoundException($"Role with ID {userDto.RoleId} not found.");
        }

        User user = _mapper.Map<User>(userDto);
        user.PasswordHash = HashPassword(userDto.Password);
        user.SetRole(userDto.RoleId);
        User? createdUser = await _userRepository.AddUserAsync(user).ConfigureAwait(false);
        if (createdUser == null)
        {
            throw new Exception("Failed to create user.");
        }

        return _mapper.Map<UserResponseDto>(createdUser);
    }

    public async Task<UserResponseDto?> UpdateUserAsync(int id, UserUpdateDto userDto)
    {
        User? user = await _userRepository.GetUserByIdAsync(id).ConfigureAwait(false);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {id} not found.");
        }

        User? existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email).ConfigureAwait(false);
        if (existingUser != null && existingUser.Id != user.Id)
        {
            throw new AlreadyExistsException($"User with email {userDto.Email} already exists.");
        }

        _mapper.Map(userDto, user);
        user.SetRole(userDto.RoleId);
        User? updatedUser = await _userRepository.UpdateUserAsync(user).ConfigureAwait(false);
        if (updatedUser == null)
        {
            throw new Exception("Failed to update user.");
        }

        return _mapper.Map<UserResponseDto>(updatedUser);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        User? user = await _userRepository.GetUserByIdAsync(id).ConfigureAwait(false);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {id} not found.");
        }

        bool success = await _userRepository.DeleteUserAsync(user).ConfigureAwait(false);
        if (!success)
        {
            throw new Exception("Failed to delete user.");
        }
        return success;
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
