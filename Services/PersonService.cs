using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestApi_CvData.Data;
using RestApi_CvData.Models;
using System.Collections;

namespace RestApi_CvData.Services
{
    public class PersonService
    {
        private readonly CvDataDBContext _context;

        public PersonService(CvDataDBContext context)
        {
            _context = context;
        }

        internal async Task<IResult> AddPerson(Person person)
        {
            if (string.IsNullOrEmpty(person.FirstName) || string.IsNullOrEmpty(person.LastName))
            {
                return Results.BadRequest("Invalid FirstName or SecondName");
            }
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
            return Results.Created($"/persons/{person.Id}", person);
        }

        internal async Task<IResult> DeletePerson(int id)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
            if (person == null)
                return Results.NotFound();

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return Results.Ok("Deleted");
        }

        internal async Task<IResult> GetAllPersonsAsync()
        {
            List<Person> list = await _context.Persons.Include(p => p.Educations).Include(p => p.WorkExperiences).ToListAsync();

            if (list.IsNullOrEmpty())
                return Results.NotFound("No Persons Found");

            return Results.Ok(list);
        }

        internal async Task<IResult> GetPersonByIdAsync(int id)
        {
            Person? person = await _context.Persons.Include(p => p.Educations).Include(p => p.WorkExperiences).FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
                return Results.NotFound("No Person Found");

            return Results.Ok(person);
        }

        internal async Task<IResult> GetPersonBySearchAsync(string firstName, string lastName, string email)
        {
            var filteredList = await _context.Persons.Include(p => p.Educations).Include(p => p.WorkExperiences).ToListAsync();

            if (!string.IsNullOrEmpty(firstName))
            {
                filteredList = filteredList.Where(p => p.FirstName.Contains(firstName)).ToList();
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                filteredList = filteredList.Where(p => p.LastName.Contains(lastName)).ToList();
            }
            if (!string.IsNullOrEmpty(email))
            {
                filteredList = filteredList.Where(p => p.Email.Contains(email)).ToList();
            }

            if (filteredList.Count > 0)
            {
                return Results.Ok(filteredList);
            }

            return Results.NotFound("No Person Found");
        }

        internal async Task<IResult> UpdatePerson(int id, Person udpatedPerson)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
            if (person == null)
                return Results.NotFound();

            person.FirstName = udpatedPerson.FirstName;
            person.LastName = udpatedPerson.LastName;
            person.Email = udpatedPerson.Email;
            person.Phone = udpatedPerson.Phone;

            await _context.SaveChangesAsync();
            return Results.Ok("Updated");
        }
    }
}
