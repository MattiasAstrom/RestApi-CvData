using System.ComponentModel.DataAnnotations;

namespace RestApi_CvData.DTOs.PersonDTOs
{
    public class UpdateWorkExperienceDTO
    {
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Company { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public string StartDate { get; set; }

        [Required]
        public string EndDate { get; set; }
    }
}
