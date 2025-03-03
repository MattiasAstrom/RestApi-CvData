
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using RestApi_CvData.Data;
using RestApi_CvData.EndPoints;
using RestApi_CvData.Models;
using RestApi_CvData.Services;

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

            builder.Services.AddScoped<PersonService>();

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
            PersonEndPoints.RegisterEndpoints(app);
            GitHubEndPoints.RegisterEndpoints(app);

            app.Run();
        }
    }
}
