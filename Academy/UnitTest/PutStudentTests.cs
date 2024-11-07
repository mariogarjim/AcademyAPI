using API;
using API.Controllers;
using API.DTO;
using API.Entities;
using API.Interfaces;
using API.Settings;
using Azure;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class PutStudentTests
    {
        private readonly Mock<ITableStorageService> stubDB;
        private readonly Mock<IHttpContextAccessor> stubCAccessor;
        private readonly Mock<ITenantSettingsFactory> stubTSettingsFac;
        private readonly Mock<IConfiguration> stubConfig;
        private readonly StudentsController alumnsController;
        private readonly string TENANT = "UniversityOfGranada";

        public PutStudentTests()
        {
            stubDB = new();
            stubCAccessor = new();
            stubTSettingsFac = new();
            stubConfig = new();

            // Setup httpcontext
            var context = new DefaultHttpContext();
            var fakeTenant = "UniversityOfGranada";
            context.Request.Headers["tenant"] = fakeTenant;
            stubCAccessor.Setup(CA => CA.HttpContext).Returns(context);
            // ---

            // Create fake list of Tenants contining the test Tenants and setup
            var testTenant = new Tenant()
            {
                Name = "University of Granada",
                TID = "UniversityOfGranada"
            };
            var testTenants = new List<Tenant>() { testTenant };
            var testTenantSettings = new TenantSettings() { Tenants = testTenants };
            stubTSettingsFac.Setup(Factory => Factory.GetTenantSettings()).Returns(Options.Create(testTenantSettings));
            // ---

            alumnsController = new(stubDB.Object, stubCAccessor.Object, stubTSettingsFac.Object);
        }

        /**
        * 
        * Success Case:
        * 1. Student Exists and is updated.
        * 
        * Fail Cases:
        * 2. Student Does not Exist;
        * 3. Student with missing fields;
        * 4. Email is in incorrect format;
        * 5. Invalid date of birth;
        * 6. Student name has forbidden characters.
        */

        // 1. Student Exists and is updated.
        [Fact]
        public async void TestPut_StudentExists_ReturnsUpdatedStudent()
        {
            // Create a new DTO for the change.
            var updatedStudent = Utilities.GetFakeAlumn();
            updatedStudent.Address = "Calle 1";
            // Methods shouldn't be called in the SetUP, just fields.
            // Therefore, pass only variables created beforehand.
            var updatedStudentGet = updatedStudent.AsGetDto();
            var updatedStudentCreate = updatedStudent.AsCreateDto();
            // Mock the presence of the entity in the Database and
            // also the update of the entity.
            stubDB.Setup(DB => DB.GetEntityAsyncById(updatedStudent.PartitionKey, updatedStudent.RowKey)).ReturnsAsync(updatedStudentGet);
            stubDB.Setup(DB => DB.UpsertEntityAsync(updatedStudentCreate, TENANT)).ReturnsAsync(updatedStudentGet);

            // Call the update method.
            var result = (OkObjectResult)await alumnsController.PutAsync(updatedStudentCreate);

            // Make assertion.
            result.Value.Should().BeEquivalentTo(updatedStudent.AsGetDto());
        }

        // 2. Student Does not Exist.
        [Fact]
        public async void TestPut_StudentDoesNotExist_ReturnsError()
        {
            // Create fake Alumn to update and mock the search by country and ID operation in the Database.
            var studentToUpdate = Utilities.GetFakeAlumn();
            stubDB.Setup(DB => DB.GetEntityAsyncById(studentToUpdate.PartitionKey, studentToUpdate.RowKey))
                .ReturnsAsync((GetAlumnDto?)null);

            // Call update operation.
            var result = (BadRequestObjectResult)await alumnsController.PutAsync(studentToUpdate.AsCreateDto());

            // Make assertion.
            result.Value.Should().Be("Student doesn't exist");
            result.StatusCode.Should().Be(400);
        }

        // 3. Student with missing fields.
        [Fact]
        public async void TestPut_IncorrectInputStudent_ReturnsBadRequest()
        {
            Alumn studentWithMissingInput = Utilities.GetAlumnWithMissingFields().AsAlumnEntity(TENANT);

            alumnsController.ModelState.AddModelError("Name", "Required");
            var result = (BadRequestResult)await alumnsController.PutAsync(studentWithMissingInput.AsCreateDto());

            result.StatusCode.Should().Be(400);
        }

        // 4. Email is in incorrect format.
        [Fact]
        public async void TestPut_IncorrectEmail_ReturnsBadRequest()
        {
            Alumn studentWithIncorrectEmail = Utilities.GetAlumnWithIncorrectEmail().AsAlumnEntity(TENANT);

            alumnsController.ModelState.AddModelError("Email", "RegularExpression");
            var result = (BadRequestResult)await alumnsController.PutAsync(studentWithIncorrectEmail.AsCreateDto());

            result.StatusCode.Should().Be(400);
        }

        // 5. Invalid date of birth.
        [Fact]
        public async void TestPut_InvalidDateOfBirth_ReturnsBadRequest()
        {
            Alumn studentWithInvalidBirthDate = Utilities.GetAlumnWithInvalidBirthDate().AsAlumnEntity(TENANT);

            var result = (BadRequestObjectResult)await alumnsController.PutAsync(studentWithInvalidBirthDate.AsCreateDto());

            result.StatusCode.Should().Be(400);
            result.Value.Should().Be("Date of birth is invalid");
        }

        // 6. Student name has forbidden characters.
        [Fact]
        public async void TestPut_ForbiddenChars_ReturnsBadRequest()
        {
            Alumn studentWithForbiddenChars = Utilities.GetAlumnWithForbiddenChars().AsAlumnEntity(TENANT);

            alumnsController.ModelState.AddModelError("Name", "RegularExpression");
            var result = (BadRequestResult)await alumnsController.PutAsync(studentWithForbiddenChars.AsCreateDto());

            result.StatusCode.Should().Be(400);
        }





    }
}
