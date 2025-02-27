using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities.Models;

[Table("roles")]
public class Role
{
    [Key, Column("id")]
    public int Id { get; set; }

    [Required, MaxLength(50), Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    // Soft delete
    [Column("status")]
    public bool Status { get; set; } = true;

    // Navigation Property
    public virtual ICollection<User> Users { get; private set; } = new List<User>();

    protected internal void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
