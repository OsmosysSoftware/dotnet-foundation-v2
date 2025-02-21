using Core.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Core.DataContext;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserLog> UserLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of Role if Users exist
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private void UpdateTimestamps()
    {
        foreach (EntityEntry entry in ChangeTracker.Entries()
                 .Where(e => e.State == EntityState.Modified))
        {
            switch (entry.Entity)
            {
                case User user:
                    user.UpdatedAt = DateTime.UtcNow;
                    break;
                case Role role:
                    role.UpdatedAt = DateTime.UtcNow;
                    break;
                case UserLog userLog:
                    userLog.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}
