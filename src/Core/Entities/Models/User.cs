using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities.Models;

[Table("users")]
[Index(nameof(Email), IsUnique = true)]
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

    // Foreign Key for Role
    [Column("role_id")]
    public int RoleId { get; private set; }

    // Navigation Property (Read-Only to avoid RoleId conflicts)
    [ForeignKey(nameof(RoleId))]
    public virtual Role? Role { get; private set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    // Soft delete
    [Column("status")]
    public bool Status { get; set; } = true;

    // Method to set RoleId to avoid navigation conflicts
    public void SetRole(int roleId)
    {
        RoleId = roleId;
        Role = null; // Ensures EF does not override RoleId
    }

    protected internal void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
