using Microsoft.Extensions.Diagnostics.HealthChecks;
using Core.DataContext;

namespace Api.HealthChecks;
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly DatabaseContext _dbContext;

    public DatabaseHealthCheck(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if the database is reachable
            if (await _dbContext.Database.CanConnectAsync(cancellationToken).ConfigureAwait(false))
            {
                return HealthCheckResult.Healthy("Database is online");
            }
            return HealthCheckResult.Unhealthy("Database is unreachable");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Database check failed: {ex.Message}");
        }
    }
}
