using Moq;
using FluentAssertions;
using API.Interfaces;
using API.Entities;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using API.DTO;
using API;
using Microsoft.AspNetCore.Http;
using API.Services;
using Microsoft.Extensions.Configuration;

namespace UnitTest
{

    public class FilterStudentsTests
    {
        private readonly Mock<ITableStorageService> stubDB;
        private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
        private readonly TenantSettingsFactory tenantSettingsFactory;
        private readonly IConfiguration configuration;
        private readonly StudentsController alumnsController;
        private readonly List<GetAlumnDto> expectedAlumns;

        public FilterStudentsTests()
        {
            //Mock IHttpContextAccessor
            mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeTenant = "UniversityOfGranada";
            context.Request.Headers["tenant"] = fakeTenant;
            mockHttpContextAccessor.Setup(contextAccessor => contextAccessor.HttpContext).Returns(context);

            //Mock IConfiguration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"tenant_configuration.json")
                .Build();

            //Mock tenantSettingsFactory
            tenantSettingsFactory = new TenantSettingsFactory(configuration);

            //Mock DB
            stubDB = new();

            //Mock Controller
            alumnsController = new(stubDB.Object, mockHttpContextAccessor.Object, tenantSettingsFactory);

            //Mock data
            expectedAlumns = new() {
                Utilities.GetFakeAlumn().AsGetDto(),
                Utilities.GetFakeAlumn().AsGetDto(),
                Utilities.GetFakeAlumn().AsGetDto()
            };
        }

        [Fact]
        public async void GetStudentByTenantAndId_WithUnexistentTenantAndId_ShouldNotReturnTheStudent()
        {
            stubDB.Setup(DB => DB.GetEntityAsyncById(It.IsAny<String>(), It.IsAny<String>()))
                  .ReturnsAsync((GetAlumnDto?)null);

            var result = await alumnsController.GetAsyncById(It.IsAny<String>());

            result.Should().BeOfType<NotFoundResult>();
        }


        [Fact]
        public async void GetStudentByTenantAndId_WithExistentTenantAndId_ShouldReturnTheStudent()
        {
            var fakeAlumn = Utilities.GetFakeAlumn().AsGetDto();

            stubDB.Setup(DB => DB.GetEntityAsyncById(fakeAlumn.University, fakeAlumn.ID)).ReturnsAsync(fakeAlumn);

            var result = await alumnsController.GetAsyncById(fakeAlumn.ID) as OkObjectResult;

            result.Should().NotBeNull();
            result?.Value.Should().BeEquivalentTo(fakeAlumn);
        }


        [Fact]
        public async void GetStudentBydId_WithUnexistentId_ShouldReturnNotFound()
        {
            stubDB.Setup(DB => DB.GetEntityAsyncById(It.IsAny<String>(), It.IsAny<String>())).ReturnsAsync((GetAlumnDto?)null);

            var result = await alumnsController.GetAsyncById(It.IsAny<String>());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void GetStudentById_WithExistentId_ShouldReturnStudent()
        {
            var fakeAlumn = Utilities.GetFakeAlumn().AsGetDto();
            stubDB.Setup(DB => DB.GetEntityAsyncById(fakeAlumn.University, fakeAlumn.ID)).ReturnsAsync(fakeAlumn);

            var result = await alumnsController.GetAsyncById(fakeAlumn.ID) as OkObjectResult;

            result.Should().NotBeNull();
            result?.Value.Should().BeEquivalentTo(fakeAlumn);
        }
    }
}