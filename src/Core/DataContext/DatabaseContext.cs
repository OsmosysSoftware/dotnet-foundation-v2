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

        modelBuilder.Entity<UserLog>()
            .HasOne(log => log.User)
            .WithMany()
            .HasForeignKey(log => log.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Global query filter for UserLogs to fetch active records
        modelBuilder.Entity<UserLog>()
            .HasQueryFilter(log => log.Status);

        /*
        // Fetches activer ecords w/o WHERE clause
        var allLogs = _context.UserLogs.ToList();

        // Query Soft-Deleted Records
        var allLogsIncludingDeleted = _context.UserLogs.IgnoreQueryFilters().ToList();
        */
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
                    user.UpdateTimestamp();
                    break;
                case Role role:
                    role.UpdateTimestamp();
                    break;
                case UserLog userLog:
                    userLog.UpdateTimestamp();
                    break;
            }
        }
    }
}
