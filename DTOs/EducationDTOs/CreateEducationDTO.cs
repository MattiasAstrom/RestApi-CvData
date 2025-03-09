using System.ComponentModel.DataAnnotations;

namespace RestApi_CvData.DTOs.EducationDTOs
{
    public class CreateEducationDTO
    {
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string SchoolName { get; set; }

        [Required]
        public string StartDate { get; set; }

        [Required]
        public string EndDate { get; set; }

        [Required]
        [MaxLength(10)]
        public string Grade { get; set; }
    }
}