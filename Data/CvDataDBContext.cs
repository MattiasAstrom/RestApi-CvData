using Microsoft.EntityFrameworkCore;
using RestApi_CvData.Models;

namespace RestApi_CvData.Data
{
    public class CvDataDBContext : DbContext
    {
        public CvDataDBContext(DbContextOptions<CvDataDBContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }
    }
}
