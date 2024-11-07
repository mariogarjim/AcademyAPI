using API.Validation;
using System.ComponentModel.DataAnnotations;


namespace API.DTO
{
    
    public class CreateAlumnDto
    {
        const int MINIMUM_LENGHT = 1;
        const int MAXIMUM_LENGHT = 50;
        const int MINIMUM_LENGHT_ID = 8;
        const int MAXIMUM_LENGHT_ID = 12;


        [Required(ErrorMessage = "Name is required.")]
        [MaxLength (MAXIMUM_LENGHT, ErrorMessage = "Too long")]
        [MinLength (MINIMUM_LENGHT, ErrorMessage = "Can't be empty")]
        [RegularExpression(RegexCreate.regexName,
         ErrorMessage = "Special characters are not allowed.")]
        
        public string Name { get; set; } = String.Empty;

        [Required(ErrorMessage = "ID is required.")]
        [MinLength (MINIMUM_LENGHT_ID, ErrorMessage = "ID minimun lenght must be 8")]
        [MaxLength(MAXIMUM_LENGHT_ID, ErrorMessage = "ID maximum lenght must be 12")]
        [RegularExpression(RegexCreate.regexID,
         ErrorMessage = "The ID must have 8-12 numbers and an optional capital letter at the end")]
        // Represents NIF/PESEL/DNI
        public string ID { get; set; } = default!;

        [Required(ErrorMessage = "Surname is required.")]
        [MaxLength(MAXIMUM_LENGHT, ErrorMessage = "Too long")]
        [MinLength(MINIMUM_LENGHT, ErrorMessage = "Can't be empty")]
        [RegularExpression(RegexCreate.regexName,
         ErrorMessage = "Special characters are not allowed.")]
        public string Surname { get; set; } = String.Empty;

        [Required(ErrorMessage = "Mail is required.")]
        [MaxLength(2*MAXIMUM_LENGHT, ErrorMessage = "Too long")]
        [MinLength(5*MINIMUM_LENGHT, ErrorMessage = "Can't be empty")]
        [RegularExpression(RegexCreate.regexEmail,
         ErrorMessage = "Not a valid email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = String.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]      
        [DataType(DataType.Date)]
        public DateTimeOffset DateOfBirth { get; set; }

        [Required(ErrorMessage = "Starting day is required.")]       
        [DataType(DataType.Date)]
        public DateTimeOffset StartingDay { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [MaxLength(MAXIMUM_LENGHT, ErrorMessage = "Too long")]
        [MinLength(MINIMUM_LENGHT, ErrorMessage = "Can't be empty")]
        [RegularExpression(RegexCreate.regexCountry,
         ErrorMessage = "Invalid characters")]
        public string Country { get; set; } = String.Empty;

        [Required(ErrorMessage = "Image is required.")]
        [MaxLength(6*MAXIMUM_LENGHT, ErrorMessage = "Too long")]
        [MinLength(MINIMUM_LENGHT, ErrorMessage = "Can't be empty")]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; } = String.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(2*MAXIMUM_LENGHT, ErrorMessage = "Too long")]
        [MinLength(MINIMUM_LENGHT, ErrorMessage = "Can't be empty")]
        [RegularExpression(@"^[a-zA-Z''-'1234567890\s]{1,100}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Address { get; set; } = String.Empty;

        [Required(ErrorMessage = "Degree is required.")]
        [MaxLength(MAXIMUM_LENGHT, ErrorMessage = "Too long")]
        [MinLength(MINIMUM_LENGHT, ErrorMessage = "Can't be empty")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,50}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Degree { get; set; } = String.Empty;

        [Required(ErrorMessage = "Course is required.")]
        [MaxLength(MAXIMUM_LENGHT, ErrorMessage = "Too long")]
        [MinLength(MINIMUM_LENGHT, ErrorMessage = "Can't be empty")]
        [RegularExpression(RegexCreate.regexName,
         ErrorMessage = "Characters are not allowed.")]
        public string Course { get; set; } = String.Empty;

    }
}
