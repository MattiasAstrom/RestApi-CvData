
using Microsoft.EntityFrameworkCore;
using RestApi_CvData.Data;
using RestApi_CvData.Models;

namespace RestApi_CvData
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<CvDataDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddHttpClient<GitHubService>(client =>
            {
                client.DefaultRequestHeaders.Add("User-Agent", "RestApi-CvData");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            //EndPoints
            app.MapGet("/persons", async (CvDataDBContext context) =>
            {
                return await context.Persons.Include(p => p.Educations).Include(p => p.WorkExperiences).ToListAsync();
            });

            app.MapGet("/persons/{id}", async (CvDataDBContext context, int id) =>
            {
                return await context.Persons.Include(p => p.Educations).Include(p => p.WorkExperiences).FirstOrDefaultAsync(p => p.Id == id) 
                is Person person ? Results.Ok() : Results.NotFound();
            });

            app.MapGet("/persons/search", async (CvDataDBContext context, string? FirstName, string? LastName, string? email) =>
            {
                var filteredList =  await context.Persons.Include(p => p.Educations).Include(p => p.WorkExperiences).ToListAsync();

                if (!string.IsNullOrEmpty(FirstName))
                {
                    filteredList = filteredList.Where(p => p.FirstName.Contains(FirstName)).ToList();
                }
                if (!string.IsNullOrEmpty(LastName))
                {
                    filteredList = filteredList.Where(p => p.LastName.Contains(LastName)).ToList();
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
            });

            app.MapPost("/persons", async (CvDataDBContext context, Person person) =>
            {
                if (string.IsNullOrEmpty(person.FirstName) || string.IsNullOrEmpty(person.LastName))
                {
                    return Results.BadRequest("Invalid FirstName or SecondName");
                }
                context.Persons.Add(person);
                await context.SaveChangesAsync();
                return Results.Created($"/persons/{person.Id}", person);
            });

            app.MapPut("/persons{id}", async (CvDataDBContext context, int id, Person udpatedPerson) =>
            {
                var person = await context.Persons.FirstOrDefaultAsync(p => p.Id == id);
                if (person == null)
                    return Results.NotFound();

                person.FirstName = udpatedPerson.FirstName;
                person.LastName = udpatedPerson.LastName;
                person.Email = udpatedPerson.Email;
                person.Phone = udpatedPerson.Phone;

                await context.SaveChangesAsync();
                return Results.Ok("Updated");
            });

            app.MapDelete("/persons/{id}", async (CvDataDBContext context, int id) =>
            {
                var person = await context.Persons.FirstOrDefaultAsync(p => p.Id == id);
                if (person == null)
                    return Results.NotFound();

                context.Persons.Remove(person);
                await context.SaveChangesAsync();
                return Results.Ok("Deleted");
            });

            app.MapGet("/github/{username}", async (string username, GitHubService gitHubService) =>
            {
                var repos = await gitHubService.GetRepositoriesAsync(username);

                if (repos == null)
                    return Results.NotFound("No repositories found.");
                else
                    return Results.Ok(repos);
            });

            app.Run();
        }
    }
}
