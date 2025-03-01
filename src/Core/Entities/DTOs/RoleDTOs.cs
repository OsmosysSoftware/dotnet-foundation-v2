using System.ComponentModel.DataAnnotations;

namespace Core.Entities.DTOs;

public abstract class RoleBaseDto
{
    [Required(ErrorMessage = "Role name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Role name must be between 2 and 50 characters")]
    public string Name { get; set; } = string.Empty;
}

public class RoleCreateDto: RoleBaseDto
{
}

public class RoleUpdateDto: RoleBaseDto
{
}

public class RoleResponseDto: RoleBaseDto
{
    public int Id { get; set; }
}