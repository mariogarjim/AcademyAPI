using API.Controllers;
using API.Interfaces;
using API.Services;
using API.Settings;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class GetTenantsTests
    {
        private readonly Mock<ITableStorageService> stubDB;
        private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
        private readonly ITenantSettingsFactory tenantSettingsFactory;
        private readonly IConfiguration configuration;
        private readonly StudentsController alumnsController;

        public GetTenantsTests()
        {
            //Mock IHttpContextAccessor
            mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeTenant = "UniversityOfGranada";
            context.Request.Headers["tenant"] = fakeTenant;
            mockHttpContextAccessor.Setup(contextAccessor => contextAccessor.HttpContext).Returns(context);

            //Mock DB
            stubDB = new();

            //Mock IConfiguration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"tenant_configuration.json")
                .Build();

            tenantSettingsFactory = new TenantSettingsFactory(configuration);
            alumnsController = new(stubDB.Object, mockHttpContextAccessor.Object, tenantSettingsFactory);
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
        public void TestGetAllTenants()
        {
            
            OkObjectResult result = (OkObjectResult)alumnsController.GetAllTenants();

            result.Value.Should().BeEquivalentTo(tenantsToReturn());
        }
    }
}
