using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestApi_CvData.Data;
using RestApi_CvData.DTOs.EducationDTOs;
using RestApi_CvData.DTOs.PersonDTOs;
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

        internal async Task<IResult> AddPerson(CreatePersonDTO person)
        {
            if (string.IsNullOrEmpty(person.FirstName) || string.IsNullOrEmpty(person.LastName))
                return Results.BadRequest("Invalid FirstName or SecondName");
            
            Person newPerson = new Person
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                Phone = person.Phone
            };

            _context.Persons.Add(newPerson);
            await _context.SaveChangesAsync();
            return Results.Created($"/persons/{newPerson.Id}", person);
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
            List<Person> list = await _context.Persons.Include(p => p.Educations)
                                                      .Include(p => p.WorkExperiences)
                                                      .ToListAsync();
            if (list.IsNullOrEmpty())
                return Results.NotFound("No Persons Found");

            return Results.Ok(list);
        }

        internal async Task<IResult> GetPersonByIdAsync(int id)
        {
            Person? person = await _context.Persons.Include(p => p.Educations).Include(p => p.WorkExperiences).FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
                return Results.NotFound("No Person Found");

            PersonDTO personDTO = new()
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                Phone = person.Phone
            };

            return Results.Ok(personDTO);
        }

        internal async Task<IResult> GetPersonBySearchAsync(string firstName, string lastName, string email)
        {
            IQueryable<Person> query = _context.Persons;

            if (firstName != null)
                query = query.Where(p => p.FirstName.Contains(firstName));
            if (lastName != null)
                query = query.Where(p => p.LastName.Contains(lastName));
            if (email != null)
                query = query.Where(p => p.Email.Contains(email));

            var filteredList = await query.ToListAsync();

            List<PersonDTO> personDTOList = filteredList.Select(person => new PersonDTO
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                Phone = person.Phone
            }).ToList();

            if (personDTOList.Count > 0)
                return Results.Ok(personDTOList);

            return Results.NotFound("No Person Found");
        }

        internal async Task<IResult> UpdatePerson(int id, PersonDTO udpatedPerson)
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

        internal async Task<IResult> AddEducation(int personId, CreateEducationDTO education)
        {
            var person = await _context.Persons.FindAsync(personId);

            if (person == null)
                return Results.NotFound("Person not found.");

            var newEducation = new Education
            {
                SchoolName = education.SchoolName,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
                Grade = education.Grade,
                FK_PersonId = personId,
                Person = person
            };

            _context.Educations.Add(newEducation);
            await _context.SaveChangesAsync();

            var educationWithPerson = await _context.Educations
                .Where(e => e.Id == newEducation.Id)
                .Include(e => e.Person)
                .FirstOrDefaultAsync();

            return Results.Created($"/persons/{personId}/educations/{newEducation.Id}", educationWithPerson);
        }

        internal async Task<IResult> AddWorkExperience(int personId, CreateWorkExperienceDTO workExperience)
        {
            var person = await _context.Persons.FindAsync(personId);

            if (person == null)
                return Results.NotFound("Person not found.");

            var newWorkExperience = new WorkExperience
            {
                Title = workExperience.Title,
                Company = workExperience.Company,
                Description = workExperience.Description,
                StartDate = workExperience.StartDate,
                EndDate = workExperience.EndDate,
                FK_PersonId = personId,
                Person = person
            };

            _context.WorkExperiences.Add(newWorkExperience);
            await _context.SaveChangesAsync();

            var workExperienceWithPerson = await _context.WorkExperiences
                .Where(w => w.Id == newWorkExperience.Id)
                .Include(w => w.Person)
                .FirstOrDefaultAsync();

            return Results.Created($"/persons/{personId}/workexperiences/{newWorkExperience.Id}", workExperienceWithPerson);
        }


        internal async Task<IResult> UpdateEducation(int personId, int educationId, UpdateEducationDTO updatedEducation)
        {
            var education = await _context.Educations.FirstOrDefaultAsync(e => e.Id == educationId && e.FK_PersonId == personId);

            if (education == null)
                return Results.NotFound("Education not found.");

            education.SchoolName = updatedEducation.SchoolName;
            education.StartDate = updatedEducation.StartDate;
            education.EndDate = updatedEducation.EndDate;
            education.Grade = updatedEducation.Grade;

            await _context.SaveChangesAsync();

            return Results.Ok("Education updated.");
        }
        internal async Task<IResult> UpdateWorkExperience(int personId, int workExperienceId, UpdateWorkExperienceDTO updatedWorkExperience)
        {
            var workExperience = await _context.WorkExperiences.FirstOrDefaultAsync(w => w.Id == workExperienceId && w.FK_PersonId == personId);

            if (workExperience == null)
                return Results.NotFound("Work experience not found.");

            workExperience.Title = updatedWorkExperience.Title;
            workExperience.Company = updatedWorkExperience.Company;
            workExperience.Description = updatedWorkExperience.Description;
            workExperience.StartDate = updatedWorkExperience.StartDate;
            workExperience.EndDate = updatedWorkExperience.EndDate;

            await _context.SaveChangesAsync();

            return Results.Ok("Work experience updated.");
        }
        internal async Task<IResult> RemoveEducation(int personId, int educationId)
        {
            var education = await _context.Educations.FirstOrDefaultAsync(e => e.Id == educationId && e.FK_PersonId == personId);

            if (education == null)
                return Results.NotFound("Education not found.");

            _context.Educations.Remove(education);
            await _context.SaveChangesAsync();

            return Results.Ok("Education removed.");
        }
        internal async Task<IResult> RemoveWorkExperience(int personId, int workExperienceId)
        {
            var workExperience = await _context.WorkExperiences.FirstOrDefaultAsync(w => w.Id == workExperienceId && w.FK_PersonId == personId);

            if (workExperience == null)
                return Results.NotFound("Work experience not found.");

            _context.WorkExperiences.Remove(workExperience);
            await _context.SaveChangesAsync();

            return Results.Ok("Work experience removed.");
        }
    }
}
