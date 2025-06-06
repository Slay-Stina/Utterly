using Utterly.Areas.Identity.Data;
using Utterly.Tools;

namespace Utterly.DAL;

public class APIManager
{
    private readonly HttpClient _httpClient;
    public APIManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://utterlyapi.azurewebsites.net/");
    }
    public async Task<List<UtterlyPost>> GetUtterlyPostsAsync()
    {
        var response = await _httpClient.GetAsync("Post");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<UtterlyPost>>() ?? new List<UtterlyPost>();
    }

    public async Task<bool> CreateUtterlyPostAsync(UtterlyPost post)
    {
        var response = await _httpClient.PostAsync("Post", post.ToContent());
        return response.IsSuccessStatusCode;
    }

    internal async Task<List<UtterlyPost>> GetUtterlyPostsByUserIdAsync(string id)
    {
        var response = await _httpClient.GetAsync($"Post/User/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return new List<UtterlyPost>();
        }
        return await response.Content.ReadFromJsonAsync<List<UtterlyPost>>() ?? new List<UtterlyPost>();
    }
}
