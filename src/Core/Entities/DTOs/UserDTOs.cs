using System.ComponentModel.DataAnnotations;

namespace Core.Entities.DTOs;

public class UserBaseDto
{
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public int RoleId { get; set; }
}

public class UserCreateDto : UserBaseDto
{
    [Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;
}

public class UserUpdateDto : UserBaseDto { }

public class UserResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class LoginRequestDto
{
    [Required, EmailAddress, MaxLength(255)]    
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}

