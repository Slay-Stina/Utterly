using Utterly.Areas.Identity.Data;
using System.Text.Json;

namespace Utterly.Services;

// Services/PostApiService.cs
public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UtterlyPost>> GetPostsAsync()
    {
        var response = await _httpClient.GetAsync("Posts");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<UtterlyPost>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    // Lägg till metoder för Create, Update, Delete etc.
}
