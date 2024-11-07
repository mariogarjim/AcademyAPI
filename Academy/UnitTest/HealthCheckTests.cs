using API;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace UnitTest
{
    
    public class HealthCheckTests
    {

        [Fact]
        public async Task HealthCheck_ShouldReturnHealthy()
        {
            HealthCheck healthCheck = new HealthCheck();

            var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());
            result.Status.Should().Be(HealthStatus.Healthy);
            result.Description.Should().Be("The app is healthy!");
            
        }
    }
    
}
