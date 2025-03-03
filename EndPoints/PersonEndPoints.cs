using Microsoft.EntityFrameworkCore;
using RestApi_CvData.Data;
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

            app.MapPost("/persons", async (CvDataDBContext context, PersonService service, Person person) =>
            {
               return await service.AddPerson(person);
            });

            app.MapPut("/persons{id}", async (CvDataDBContext context, PersonService service, int id, Person udpatedPerson) =>
            {
                return await service.UpdatePerson(id, udpatedPerson);
            });

            app.MapDelete("/persons/{id}", async (CvDataDBContext context, PersonService service, int id) =>
            {
                return await service.DeletePerson(id);
            });
        }
    }
}
