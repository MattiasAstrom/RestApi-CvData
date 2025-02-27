namespace RestApi_CvData.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ICollection<Education> Educations { get; set; }
        public ICollection<WorkExperience> WorkExperiences { get; set; }
    }
}
