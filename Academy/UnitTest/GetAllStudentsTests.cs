using Moq;
using FluentAssertions;
using API.Interfaces;
using API.Entities;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using API.DTO;
using API;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using API.Services;
using API.Settings;
using Castle.Components.DictionaryAdapter.Xml;

namespace UnitTest
{

    public class GetAllStudentTests
    {
        private readonly Mock<ITableStorageService> stubDB;
        private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
        private readonly TenantSettingsFactory tenantSettingsFactory;
        private readonly IConfiguration configuration;
        private readonly StudentsController alumnsController;
        private readonly List<GetAlumnDto> expectedAlumns;


        public GetAllStudentTests()
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
        public async void Get_all_students_for_the_tenant_should_return_all_students_from_the_tenant()
        {
            //Setup DB
            stubDB
                .Setup(DB => DB.GetAllEntityAsync("UniversityOfGranada"))
                .ReturnsAsync(expectedAlumns);

            //Assert
            var result = await alumnsController.GetAllAsync() as OkObjectResult;

            result.Should().NotBeNull();
            result?.Value.Should().BeEquivalentTo(expectedAlumns);

        }

        [Fact]
        public async void Get_all_students_for_different_tenant_should_returns_0_students()
        {
            //Setup DB
            stubDB
                .Setup(DB => DB.GetAllEntityAsync("UniversityOfLisbon"))
                .ReturnsAsync(expectedAlumns); 

            //Assert
            var result = (NotFoundResult) await alumnsController.GetAllAsync();

            result.StatusCode.Should().Be(404);

        }



    }
}