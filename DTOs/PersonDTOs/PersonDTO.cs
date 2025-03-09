using System.ComponentModel.DataAnnotations;

namespace RestApi_CvData.DTOs.PersonDTOs
{
    public class PersonDTO
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
    }
}
