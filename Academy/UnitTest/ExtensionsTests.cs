using Moq;
using FluentAssertions;
using API.Controllers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.DTO;
using API;
using API.Entities;

namespace UnitTest
{
    
    public class ExtensionsTests
    {

        [Fact]
        public void TestExtensions_AsCreateDto()
        {
            Alumn studentToTest = Utilities.GetFakeAlumn();
            CreateAlumnDto resultAlumnDto = studentToTest.AsCreateDto();

            resultAlumnDto.Should().BeEquivalentTo(
                studentToTest,
                options => options.ComparingByMembers<CreateAlumnDto>()
                                  .WithMapping("RowKey", "ID")
                                  .ExcludingMissingMembers()
           );
           
        }
        
        [Fact]
        public void TestExtensions_AsAlumnEntity()
        {
            CreateAlumnDto dtoToTest = Utilities.GetFakeReadAlumnDto();
            Alumn resultAlumn = dtoToTest.AsAlumnEntity("UniversityOfGranada");

            dtoToTest.Should().BeEquivalentTo(
                resultAlumn,
                options => options.ComparingByMembers<Alumn>()
                                  .WithMapping("RowKey", "ID")
                                  .ExcludingMissingMembers()
           );
            
        }

        [Fact]
        public void TestExtensions_AsGetDto()
        {
            Alumn studentToTest = Utilities.GetFakeAlumn();
            GetAlumnDto resultAlumnDto = studentToTest.AsGetDto();

            resultAlumnDto.Should().BeEquivalentTo(
                studentToTest,
                options => options.ComparingByMembers<CreateAlumnDto>()
                                  .WithMapping("RowKey", "ID")
                                  .ExcludingMissingMembers()
           );
        }

    }
    
}


