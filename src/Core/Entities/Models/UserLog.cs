using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Core.Entities.Models;

[Table("user_logs")]
public class UserLog
{
    [Key, Column("id")]
    public int Id { get; set; }

    [Required, Column("action")]
    public string Action { get; set; } = string.Empty; // LOGIN, PASSWORD_RESET, etc.

    [Column("user_id")]
    public int UserId { get; set; }
    
    // Navigation Property (Read-Only to avoid UserId conflicts)
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; private set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    // Soft delete
    [Column("status")]
    public bool Status { get; set; } = true;

    protected internal void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
