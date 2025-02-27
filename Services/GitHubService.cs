using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class GitHubService
{
    private readonly HttpClient _httpClient;

    public GitHubService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<GitHubRepo>> GetRepositoriesAsync(string username)
    {
        string path = $"https://api.github.com/users/{username}/repos";
        var response = await _httpClient.GetStringAsync(path);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var repos = JsonSerializer.Deserialize<List<GitHubRepo>>(response, options);

        if (repos != null)
            CheckForNullValues(repos);

        return repos;
    }

    //started with assigned default values in the GutHubRepo class however the serializer overrides them with null values so no we just override null values here.
    private void CheckForNullValues(List<GitHubRepo> list)
    {
        foreach (var repo in list)
        {
            if (repo.Description == null)
            {
                repo.Description = "No description provided";
            }
            if (repo.Language == null)
            {
                repo.Language = "Unkown";
            }
        }
    }
}

public class GitHubRepo
{
    public string Name { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public string Html_url { get; set; }
    public Owner Owner { get; set; }
}

public class Owner
{
    public string Login { get; set; }
}