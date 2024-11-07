using API.Interfaces;
using API.Services;
using API.Settings;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class TenantSettingsFactoryTests
    {
        private readonly Mock<IConfiguration> iConfigMock;
        private readonly ITenantSettingsFactory tenantFactory;

        public TenantSettingsFactoryTests()
        {
            iConfigMock = new Mock<IConfiguration>();
            tenantFactory = new TenantSettingsFactory(iConfigMock.Object);
        }

        static List<Tenant> tenantsToReturn()
        {
            List<Tenant> listTenants = new List<Tenant>();
            
            Tenant tenant = new Tenant();
            tenant.Name = "University of Lisbon";
            tenant.TID = "UniversityOfLisbon";

            Tenant tenant2 = new Tenant();
            tenant2.Name = "University of Paris";
            tenant2.TID = "UniversityOfParis";

            Tenant tenant3 = new Tenant();
            tenant3.Name = "University of Warsaw";
            tenant3.TID = "UniversityOfWarsaw";

            Tenant tenant4 = new Tenant();
            tenant4.Name = "University of Granada";
            tenant4.TID = "UniversityOfGranada";

            listTenants.Add(tenant);
            listTenants.Add(tenant2);
            listTenants.Add(tenant3);
            listTenants.Add(tenant4);

            return listTenants;
        }

        [Fact]
        public void TestGetTenantSettings()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("tenant_configuration.json")
            .Build();
            Mock<ConfigurationSection> outputOfGetSection = new Mock<ConfigurationSection>(configurationRoot, "Tenants");
            iConfigMock.Setup(_config => (_config.GetSection("Tenants"))).Returns(outputOfGetSection.Object);

            IOptions<TenantSettings> result = tenantFactory.GetTenantSettings();

            // result is of type IOptions<TenantSettings>
            // this is a wrapper of TenantSettings
            // result.Value returns the object TenantSettings. which has an attribute Tenants
            result.Value.Tenants.Should().BeEquivalentTo(tenantsToReturn());
        }
    }
}
