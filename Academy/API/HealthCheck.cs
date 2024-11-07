using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace API
{
    public class HealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
                                                        CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Healthy("The app is healthy!"));           
        }
    }
}
