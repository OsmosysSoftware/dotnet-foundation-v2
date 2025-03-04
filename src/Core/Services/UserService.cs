using Core.Entities.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Core.Entities.DTOs;
using AutoMapper;

namespace Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        User? user = await _userRepository.GetUserByIdAsync(id).ConfigureAwait(false);
        return user == null ? null : _mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
    {
        User? user = await _userRepository.GetUserByEmailAsync(email).ConfigureAwait(false);
        return user == null ? null : _mapper.Map<UserResponseDto>(user);
    }

    public async Task<int> GetTotalUsersCountAsync()
    {
        return await _userRepository.GetTotalUsersCountAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync(int pageNumber, int pageSize)
    {
        IEnumerable<User> users = await _userRepository.GetAllUsersAsync(pageNumber, pageSize).ConfigureAwait(false);
        return _mapper.Map<IEnumerable<UserResponseDto>>(users);
    }

    public async Task<UserResponseDto?> AddUserAsync(UserCreateDto userDto)
    {
        User? existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email).ConfigureAwait(false);
        if (existingUser != null)
        {
            return null;
        }

        User user = _mapper.Map<User>(userDto);
        user.PasswordHash = HashPassword(userDto.Password);
        user.SetRole(userDto.RoleId);
        User? createdUser = await _userRepository.AddUserAsync(user).ConfigureAwait(false);
        return createdUser == null ? null : _mapper.Map<UserResponseDto>(createdUser);
    }

    public async Task<UserResponseDto?> UpdateUserAsync(int id, UserUpdateDto userDto)
    {
        User? user = await _userRepository.GetUserByIdAsync(id).ConfigureAwait(false);
        if (user == null)
        {
            return null;
        }

        User? existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email).ConfigureAwait(false);
        if (existingUser != null && existingUser.Id != user.Id)
        {
            return null;
        }

        _mapper.Map(userDto, user);
        user.SetRole(userDto.RoleId);
        User? updatedUser = await _userRepository.UpdateUserAsync(user).ConfigureAwait(false);
        return updatedUser == null ? null : _mapper.Map<UserResponseDto>(updatedUser);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        User? user = await _userRepository.GetUserByIdAsync(id).ConfigureAwait(false);
        if (user == null)
        {
            return false;
        }

        return await _userRepository.DeleteUserAsync(user).ConfigureAwait(false);
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
