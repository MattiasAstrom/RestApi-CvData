using Microsoft.EntityFrameworkCore;
using RestApi_CvData.Data;
using RestApi_CvData.DTOs.EducationDTOs;
using RestApi_CvData.DTOs.PersonDTOs;
using RestApi_CvData.Models;
using RestApi_CvData.Services;

namespace RestApi_CvData.EndPoints
{
    public class PersonEndPoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("/persons", async (CvDataDBContext context, PersonService service) =>
            {
                return await service.GetAllPersonsAsync();
            });

            app.MapGet("/persons/{id}", async (CvDataDBContext context, PersonService service, int id) =>
            {
                return await service.GetPersonByIdAsync(id);
            });

            app.MapGet("/persons/search", async (CvDataDBContext context, PersonService service, string? FirstName, string? LastName, string? email) =>
            {
                return await service.GetPersonBySearchAsync(FirstName, LastName, email);
            });

            app.MapPost("/persons", async (CvDataDBContext context, PersonService service, CreatePersonDTO person) =>
            {
               return await service.AddPerson(person);
            });

            app.MapPut("/persons{id}", async (CvDataDBContext context, PersonService service, int id, PersonDTO udpatedPerson) =>
            {
                return await service.UpdatePerson(id, udpatedPerson);
            });

            app.MapDelete("/persons/{id}", async (CvDataDBContext context, PersonService service, int id) =>
            {
                return await service.DeletePerson(id);
            });
            app.MapPost("/persons/{personId}/educations", async (CvDataDBContext context, PersonService service, int personId, CreateEducationDTO education) =>
            {
                return await service.AddEducation(personId, education);
            });

            app.MapPut("/persons/{personId}/educations/{educationId}", async (CvDataDBContext context, PersonService service, int personId, int educationId, UpdateEducationDTO updatedEducation) =>
            {
                return await service.UpdateEducation(personId, educationId, updatedEducation);
            });

            app.MapDelete("/persons/{personId}/educations/{educationId}", async (CvDataDBContext context, PersonService service, int personId, int educationId) =>
            {
                return await service.RemoveEducation(personId, educationId);
            });

            app.MapPost("/persons/{personId}/workexperiences", async (CvDataDBContext context, PersonService service, int personId, CreateWorkExperienceDTO workExperience) =>
            {
                return await service.AddWorkExperience(personId, workExperience);
            });

            app.MapPut("/persons/{personId}/workexperiences/{workExperienceId}", async (CvDataDBContext context, PersonService service, int personId, int workExperienceId, UpdateWorkExperienceDTO updatedWorkExperience) =>
            {
                return await service.UpdateWorkExperience(personId, workExperienceId, updatedWorkExperience);
            });

            app.MapDelete("/persons/{personId}/workexperiences/{workExperienceId}", async (CvDataDBContext context, PersonService service, int personId, int workExperienceId) =>
            {
                return await service.RemoveWorkExperience(personId, workExperienceId);
            });
        }
    }
}
