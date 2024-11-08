using Moq;
using FluentAssertions;
using API.Controllers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.DTO;
using API;
using API.Entities;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace UnitTest
{
    
    public class PostStudentTests
    {
        private readonly Mock<ITableStorageService> stubDB;
        private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
        private readonly TenantSettingsFactory tenantSettingsFactory;
        private readonly IConfiguration configuration;
        private readonly StudentsController alumnsController;
        public PostStudentTests()
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
        public async void TestPost_CorrectStudent_ReturnsCreatedStudent()
        {
            var studentToCreate = Utilities.GetFakeAlumn();
            var studentDTO = studentToCreate.AsCreateDto();

            stubDB.Setup(DB => DB.GetEntityAsyncById(studentToCreate.PartitionKey, studentToCreate.RowKey)).ReturnsAsync((GetAlumnDto?)null);
            stubDB.Setup(DB => DB.UpsertEntityAsync(studentDTO, studentToCreate.PartitionKey)).ReturnsAsync(studentToCreate.AsGetDto());

            var result = (CreatedAtActionResult)await alumnsController.PostAsync(studentDTO);

            result.StatusCode.Should().Be(201);
            result.Value.Should().BeEquivalentTo(
                studentToCreate.AsGetDto(),
                options => options
                    .ComparingByMembers<CreateAlumnDto>()
            );

            var createdStudent = result.Value as Alumn;
            createdStudent?.RowKey.Should().NotBeNull();

        }
        
        [Fact]
        public async void TestPost_IncorrectInputStudent_ReturnsBadRequest()
        {
            Alumn studentWithMissingInput = Utilities.GetAlumnWithMissingFields().AsAlumnEntity("UniversityOfGranada");

            alumnsController.ModelState.AddModelError("Name", "Required");

            var result = (BadRequestResult)await alumnsController.PostAsync(studentWithMissingInput.AsCreateDto());
            result.StatusCode.Should().Be(400);
        }
        
        
        [Fact]
        public async void TestPost_ExistingStudent_ReturnsBadRequest()
        {
            Alumn duplicatedAlumn = Utilities.GetFakeAlumn();

            stubDB.Setup(DB => DB.GetEntityAsyncById(duplicatedAlumn.PartitionKey, duplicatedAlumn.AsCreateDto().ID))
                  .ReturnsAsync(duplicatedAlumn.AsGetDto());

            var result = (BadRequestObjectResult)await alumnsController.PostAsync(duplicatedAlumn.AsCreateDto());

            result.Value.Should().Be("Student already exists");
            result.StatusCode.Should().Be(400);
        }
        

        [Fact]
        public async void TestPost_IncorrectEmail_ReturnsBadRequest()
        {
            Alumn studentWithIncorrectEmail = Utilities.GetAlumnWithIncorrectEmail().AsAlumnEntity("UniversityOfGranada") ;

            alumnsController.ModelState.AddModelError("Email", "RegularExpression");

            var result = (BadRequestResult)await alumnsController.PostAsync(studentWithIncorrectEmail.AsCreateDto());

            result.StatusCode.Should().Be(400);
        }
        
        [Fact]
        public async void TestPost_ForbiddenChars_ReturnsBadRequest()
        {
            Alumn studentWithForbiddenChars = Utilities.GetAlumnWithForbiddenChars().AsAlumnEntity("UniversityOfGranada");

            alumnsController.ModelState.AddModelError("Name", "RegularExpression");

            var result = (BadRequestResult)await alumnsController.PostAsync(studentWithForbiddenChars.AsCreateDto());

            result.StatusCode.Should().Be(400);
        }

        
    }
    
}

