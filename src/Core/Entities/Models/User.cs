using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Models;

[Table("users")]
public class User
{
    [Key, Column("id")]
    public int Id { get; set; }

    [Required, MaxLength(100), Column("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100), Column("last_name")]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(255), Column("email")]
    public string Email { get; set; } = string.Empty;

    [Required, Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("role_id")]
    public int RoleId { get; set; }

    [ForeignKey("RoleId")]
    public Role? Role { get; set; } = null;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}