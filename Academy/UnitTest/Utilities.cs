using API.DTO;
using API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    internal class Utilities
    {
        public static Alumn GetFakeAlumn()
        {
            return new()
            {
                PartitionKey = "UniversityOfGranada",
                RowKey = "24342123T",
                Name = "Mario",
                Surname = "Garcia Jimenez",
                Address = "Calle Sol 3",
                Course = "Maths",
                DateOfBirth = new DateTime(2001, 10, 7),
                Degree = "Computer Science",
                Email = "mario.garcia@unit4.com",
                Image = "image.jpg",
                StartingDay = new DateTime(2022, 12, 8),
                Country = "Spain",
                Timestamp = DateTime.Now,
                ETag = new Azure.ETag()
            };
        }

        public static CreateAlumnDto GetAlumnWithMissingFields()
        {
            return new()
            {
                ID = "24342123T",
                Surname = "Garcia Jimenez",
                Address = "Calle Sol 3",
                Country = "Spain",
                Course = "Maths",
                DateOfBirth = new DateTime(2001, 10, 7),
                Degree = "Computer Science",
                Email = "mario.garcia@unit4.com",
                Image = "image.jpg",
                StartingDay = new DateTime(2023, 12, 8),
                
            };
        }

        public static CreateAlumnDto GetAlumnWithIncorrectEmail()
        {
            return new()
            {
                ID = "24342123T",
                Name = "Mario",
                Surname = "Garcia Jimenez",
                Address = "Calle Sol 3",
                Country = "Spain",
                Course = "Maths",
                DateOfBirth = new DateTime(2001, 10, 7),
                Degree = "Computer Science",
                Email = "mario.garciaunit4.com",
                Image = "image.jpg",
                StartingDay = new DateTime(2023, 12, 8),
            };
        }
        public static CreateAlumnDto GetAlumnWithInvalidBirthDate()
        {
            return new()
            {
                ID = "24342123T",
                Name = "Mario",
                Surname = "Garcia Jimenez",
                Address = "Calle Sol 3",
                Country = "Spain",
                Course = "Maths",
                DateOfBirth = new DateTime(2023, 12, 8),
                Degree = "Computer Science",
                Email = "mario.garciaunit4.com",
                Image = "image.jpg",
                StartingDay = new DateTime(2023, 12, 8),
            };
        }
        public static CreateAlumnDto GetAlumnWithForbiddenChars()
        {
            return new()
            {
                ID = "24342123T",
                Name = "SELECT * FROM ALUMN",
                Surname = "Garcia Jimenez",
                Address = "Calle Sol 3",
                Country = "Spain",
                Course = "Maths",
                DateOfBirth = new DateTime(2001, 10, 7),
                Degree = "Computer Science",
                Email = "mario.garciaunit4.com",
                Image = "image.jpg",
                StartingDay = new DateTime(2023, 12, 8),
            };
        }

        public static GetAlumnDto GetFakeGetAlumnDto()
        {
            return new()
            {
                ID = "24342123T",
                Name = "Mario",
                Surname = "Garcia Jimenez",
                Address = "Calle Sol 3",
                Country = "Spain",
                Course = "Maths",
                DateOfBirth = new DateTime(2001, 10, 7),
                Degree = "Computer Science",
                Email = "mario.garcia@unit4.com",
                Image = "image.jpg",
                StartingDay = new DateTime(2023, 12, 8),
                //University = "UniversityOfGranada"
            };
        }

        public static CreateAlumnDto GetFakeReadAlumnDto()
        {
            return new()
            {
                ID = "24342123T",
                Name = "Mario",
                Surname = "Garcia Jimenez",
                Address = "Calle Sol 3",
                Country = "Spain",
                Course = "Maths",
                DateOfBirth = new DateTime(2001, 10, 7),
                Degree = "Computer Science",
                Email = "mario.garcia@unit4.com",
                Image = "image.jpg",
                StartingDay = new DateTime(2023, 12, 8),
                
            };
        }

    }
}
