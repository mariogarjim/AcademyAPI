using API.DTO;
using API.Entities;

namespace API
{
    public static class Extensions
    {

        public static Alumn AsAlumnEntity(this CreateAlumnDto alumnDto, string tenant)
        {
            return new Alumn
            {
                PartitionKey = tenant,
                RowKey = alumnDto.ID,
                Name = alumnDto.Name,
                Surname = alumnDto.Surname,
                Country = alumnDto.Country,
                StartingDay = alumnDto.StartingDay,
                Email = alumnDto.Email,
                Image = alumnDto.Image,
                Degree = alumnDto.Degree,
                DateOfBirth = alumnDto.DateOfBirth,
                Address = alumnDto.Address,
                Course = alumnDto.Course,
                Timestamp = DateTimeOffset.Now



            };
        }

        public static CreateAlumnDto AsCreateDto(this Alumn alumn)
        {
            return new CreateAlumnDto
            {
                ID = alumn.RowKey,
                Name = alumn.Name,
                Surname = alumn.Surname,
                Country = alumn.Country,
                StartingDay = alumn.StartingDay,
                Email = alumn.Email,
                Image = alumn.Image,
                Degree = alumn.Degree,
                DateOfBirth = alumn.DateOfBirth,
                Address = alumn.Address,
                Course = alumn.Course
            };
        }

        public static GetAlumnDto AsGetDto(this Alumn alumn)
        {
            return new GetAlumnDto
            {
                University = alumn.PartitionKey,
                ID = alumn.RowKey,
                Name = alumn.Name,
                Surname = alumn.Surname,
                Country = alumn.Country,
                StartingDay = alumn.StartingDay,
                Email = alumn.Email,
                Image = alumn.Image,
                Degree = alumn.Degree,
                DateOfBirth = alumn.DateOfBirth,
                Address = alumn.Address,
                Course = alumn.Course
            };
        }
    }



}