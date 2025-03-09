using System.Text.Json.Serialization;

namespace RestApi_CvData.Models
{
    public class Education
    {
        public int Id { get; set; }

        public string SchoolName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Grade { get; set; }


        public int FK_PersonId { get; set; }
        [JsonIgnore]
        public Person? Person { get; set; }
    }
}
