namespace Core.Entities.DTOs;

public class RoleCreateDto
{
    public string Name { get; set; } = string.Empty;
}

public class RoleUpdateDto
{
    public string Name { get; set; } = string.Empty;
}

public class RoleResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}