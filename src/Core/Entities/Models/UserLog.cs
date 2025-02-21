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

    [ForeignKey("User"), Column("user_id")]
    public int UserId { get; set; }
    public User? User { get; set; } = null;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
