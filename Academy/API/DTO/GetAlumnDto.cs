using System.ComponentModel.DataAnnotations;



namespace API.DTO
{

    public class GetAlumnDto
    {

        public string Name { get; set; } = String.Empty;
        // Represents NIF/PESEL/DNI
        public string ID { get; set; } = default!;
        public string Surname { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public DateTimeOffset DateOfBirth { get; set; }
        public DateTimeOffset StartingDay { get; set; }
        public string Country { get; set; } = String.Empty;
        public string Image { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;  
        public string University { get; set; } = String.Empty;
        public string Degree { get; set; } = String.Empty;
        public string Course { get; set; } = String.Empty;

    }
}
