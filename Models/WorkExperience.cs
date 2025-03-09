using System.Text.Json.Serialization;

namespace RestApi_CvData.Models
{
    public class WorkExperience
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public int FK_PersonId { get; set; }
        [JsonIgnore]
        public Person? Person { get; set; }
    }
}
