using API.Controllers;
using API.DTO;
using API.Entities;
using API.Interfaces;
using API.Services;
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
    public class DeleteStudentTests
    {
        private readonly Mock<ITableStorageService> stubDB;
        private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
        private readonly TenantSettingsFactory tenantSettingsFactory;
        private readonly IConfiguration configuration;
        private readonly StudentsController alumnsController;
        public DeleteStudentTests()
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

        }

        [Fact]
        public async void TestDel_CorrectStudent_ReturnsOk()
        {
            Alumn alumnToDelete = Utilities.GetFakeAlumn();
            stubDB.Setup(DB => DB.GetEntityAsyncById(alumnToDelete.PartitionKey, alumnToDelete.RowKey)).ReturnsAsync(Utilities.GetFakeGetAlumnDto());
            var result = (NoContentResult) await alumnsController.DeleteAsync(alumnToDelete.RowKey);

            stubDB.Verify(r => r.DeleteEntityAsync(alumnToDelete.PartitionKey, alumnToDelete.RowKey));
            result.StatusCode.Should().Be(204);          
        }

        [Fact]
        public async void TestDel_NotExistingStudent_ReturnsBadRequest()
        {
            Alumn alumnToDelete = Utilities.GetFakeAlumn();
            stubDB.Setup(DB => DB.GetEntityAsyncById(alumnToDelete.PartitionKey, alumnToDelete.RowKey)).ReturnsAsync((GetAlumnDto?)null);
            var result = (BadRequestObjectResult) await alumnsController.DeleteAsync(alumnToDelete.RowKey);
            result.Value.Should().Be("Student doesn't exist");
            result.StatusCode.Should().Be(400);
        }
    }
    
}
