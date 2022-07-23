using Bets.Infrastructure.Repositories;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace Bets.API.HealthCheck
{
    public class BetsCheck : IHealthCheck
    {
        private readonly BetsConnectionConfig _connectionConfig;

        public BetsCheck(IOptions<BetsConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig.Value ?? throw new ArgumentNullException(nameof(connectionConfig));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using (var cnn = new SqlConnection(_connectionConfig.BetsConnectionString))
            {
                try
                {
                    await cnn.OpenAsync();
                }
                catch (SqlException)
                {
                    return await Task.FromResult(HealthCheckResult.Unhealthy("Sql connection error"));
                }
            }
            return await Task.FromResult(HealthCheckResult.Healthy("Ok"));
        }
    }
}
