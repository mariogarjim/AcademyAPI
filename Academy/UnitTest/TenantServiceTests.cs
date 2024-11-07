using API.Exceptions;
using API.Services;
using API.Settings;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
    public class TenantServiceTests
    {
        private readonly Mock<IOptions<TenantSettings>> tenantSettingsMock;
        private readonly Mock<IHttpContextAccessor> httpMock;
        


        public TenantServiceTests()
        {
            tenantSettingsMock = new();
            httpMock = new Mock<IHttpContextAccessor>();

            // SETUP THE TENANTSETTINGSMOCK
            // #######################################################################

            // We create a fake IConfiguration and set it up
            Mock<IConfiguration> iConfigMock = new();
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("tenant_configuration.json").Build();
            Mock<ConfigurationSection> outputOfGetSection = new Mock<ConfigurationSection>(configurationRoot, "Tenants");
            iConfigMock.Setup(_config => (_config.GetSection("Tenants"))).Returns(outputOfGetSection.Object);

            // We create a fake TenantSettings to pass to our mock of IOptions<TenantSettings>
            TenantSettings fakeTenantSettings = new TenantSettings();
            fakeTenantSettings.Tenants = iConfigMock.Object.GetSection("Tenants").Get<List<Tenant>>();

            tenantSettingsMock.Setup(tsm => tsm.Value).Returns(fakeTenantSettings);
            // #######################################################################

            
        }


        [Fact]
        public void TestGetTenant_WhenInitialized_SHouldReturnCurrentTenant()
        {
            //setup HttpMock
            var context = new DefaultHttpContext();
            string fakeTenant = "UniversityOfGranada";
            context.Request.Headers["tenant"] = fakeTenant;
            httpMock.Setup(contextAccessor => contextAccessor.HttpContext).Returns(context);
            TenantService tenantService = new TenantService(tenantSettingsMock.Object, httpMock.Object);

            Tenant result = tenantService.GetTenant();

            result.TID.Should().Be("UniversityOfGranada");

        }

        [Fact]
        public void TestTenantService_WithoutTenant_ShouldThrowTenantNullException()
        {
            var context = new DefaultHttpContext();
            httpMock.Setup(contextAccessor => contextAccessor.HttpContext).Returns(context);

            var constructTenantServiceAction = () => new TenantService(tenantSettingsMock.Object, httpMock.Object);
            constructTenantServiceAction.Should().Throw<TenantNullException>();
        }


        [Fact]
        public void TestTenantService_WithUnexistentTenant_ShouldThrowsKeyException()
        {
            
            //setup HttpMock
            var context = new DefaultHttpContext();
            string fakeTenant = "UniversidadLola";
            context.Request.Headers["tenant"] = fakeTenant;
            httpMock.Setup(contextAccessor => contextAccessor.HttpContext).Returns(context);
            
            var constructTenantServiceAction = () => new TenantService(tenantSettingsMock.Object, httpMock.Object);

            constructTenantServiceAction.Should().Throw<KeyNotFoundException>().WithMessage("Invalid Tenant!");

        }

        [Fact]
        public void TestTenantService_WithExistentTenant_ShouldCreateTenantService()
        {

            //setup HttpMock
            var context = new DefaultHttpContext();
            string fakeTenant = "UniversityOfGranada";
            context.Request.Headers["tenant"] = fakeTenant;
            httpMock.Setup(contextAccessor => contextAccessor.HttpContext).Returns(context);

            var tenantService = new TenantService(tenantSettingsMock.Object, httpMock.Object);

           tenantService.Should().NotBeNull();

        }



    }
}
