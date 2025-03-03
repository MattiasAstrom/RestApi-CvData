namespace RestApi_CvData.EndPoints
{
    public class GitHubEndPoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("/github/{username}", async (string username, GitHubService gitHubService) =>
            {
                var repos = await gitHubService.GetRepositoriesAsync(username);

                if (repos == null)
                    return Results.NotFound("No repositories found.");
                else
                    return Results.Ok(repos);
            });
        }
    }
}
