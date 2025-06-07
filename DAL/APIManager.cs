using Utterly.Areas.Identity.Data;
using Utterly.Tools;

namespace Utterly.DAL;

public class APIManager
{
    private readonly HttpClient _httpClient;
    private readonly UtterlyContext _utterlyContext;
    public APIManager(HttpClient httpClient, UtterlyContext utterlyContext)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://utterlyapi.azurewebsites.net/");
        _utterlyContext = utterlyContext;
    }
    public async Task<List<UtterlyPost>> GetUtterlyPostsAsync()
    {
        List<UtterlyPost> utterlyPosts = new List<UtterlyPost>();
        var response = await _httpClient.GetAsync("Post");
        response.EnsureSuccessStatusCode();
        utterlyPosts = await response.Content.ReadFromJsonAsync<List<UtterlyPost>>() ?? new List<UtterlyPost>();

        // Hämta User-objektet för varje post
        var userIds = utterlyPosts.Select(p => p.UserId).Distinct().ToList();
        var users = _utterlyContext.Users
            .Where(u => userIds.Contains(u.Id))
            .ToDictionary(u => u.Id);

        foreach (var post in utterlyPosts)
        {
            if (users.TryGetValue(post.UserId, out var user))
            {
                post.User = user;
            }
        }
        return utterlyPosts;
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

    internal async Task<List<UtterlyPost>> GetPostsByThreadIdAsync(int threadId)
    {
        var response = await _httpClient.GetAsync($"Post/Thread/{threadId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to fetch posts for the specified thread.");
        }
        var posts = await response.Content.ReadFromJsonAsync<List<UtterlyPost>>();

        return posts;
    }
}
