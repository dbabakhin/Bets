using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Bets.API.HealthCheck
{
    public class BetsCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {

            return Task.FromResult(HealthCheckResult.Healthy("Ok"));
        }
    }
}
