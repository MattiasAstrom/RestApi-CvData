
using Microsoft.EntityFrameworkCore;
using RestApi_CvData.Data;
using RestApi_CvData.EndPoints;
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

            
            //builder.Services.AddCors((options) =>
            //{
            //    options.AddPolicy("AllowAll", (builder) =>
            //    {
            //        builder.WithOrigins("http://localhost:5173/")
            //               .AllowAnyMethod()
            //               .AllowAnyHeader();
            //    });
            //});


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            //app.UseCors("AllowAll");


            //app.MapGet("/JsonDataDemo", async () =>
            //{
            //    HttpClient client = new HttpClient();
            //    //get data from external api
            //    string path = "https://jsonplaceholder.typicode.com/todos";
            //    var response = await client.GetAsync(path);


            //    if (response.IsSuccessStatusCode.Equals(true))
            //    {
            //        //parse data
            //        var json = await response.Content.ReadAsStringAsync();

            //        var options = new JsonSerializerOptions
            //        {
            //            PropertyNameCaseInsensitive = true
            //        };
            //        var repos = JsonSerializer.Deserialize<List<TodoDTO>>(json, options);


            //        //return data
            //        return Results.Ok(repos);
            //    }
            //    else
            //    {
            //        return Results.NotFound("No data found.");
            //    }
            //});

            //app.MapGet("/JsonDataDemo", async (TodoDTO todo) =>
            //{
            //    HttpClient client = new HttpClient();
            //    string path = "https://jsonplaceholder.typicode.com/todos";

            //    var json = JsonSerializer.Serialize(todo);
            //    var content = new StringContent(json, Encoding.UTF8, "application/json");
            //    var response = await client.PostAsync(path, content);

            //    if (response.IsSuccessStatusCode.Equals(false))
            //    {
            //        return Results.BadRequest();
            //    }

            //    return Results.Ok();
            //});


            //app.Use(async (HttpContext context, RequestDelegate next) =>
            //{
            //    string? configKey = builder.Configuration["ApiKey"];
            //    var apiKey = context.Request.Headers["X-API-Key"].FirstOrDefault();

            //    if (apiKey != configKey)
            //    {
            //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //        await context.Response.WriteAsync("Invalid API Key");
            //        return;
            //    }

            //    await next(context);
            //});

            //EndPoints
            PersonEndPoints.RegisterEndpoints(app);
            GitHubEndPoints.RegisterEndpoints(app);

            app.Run();
        }
    }
}

//class TodoDTO
//{
//    public int? UserId { get; set; }
//    public string? Title { get; set; }
//}