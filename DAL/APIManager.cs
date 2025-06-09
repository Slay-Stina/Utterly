using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Utterly.Areas.Identity.Data;
using Utterly.Tools;

namespace Utterly.DAL;

public class APIManager
{
    private readonly HttpClient _httpClient;
    private readonly UtterlyContext _utterlyContext;
    private readonly UserManager<UtterlyUser> _userManager;
    public APIManager(
        HttpClient httpClient, 
        UtterlyContext utterlyContext,
        UserManager<UtterlyUser> userManager)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://utterlyapi.azurewebsites.net/");
        _utterlyContext = utterlyContext;
        _userManager = userManager;
    }
    public async Task<List<UtterlyPost>> GetPostsAsync()
    {
        List<UtterlyPost> posts = new List<UtterlyPost>();
        var response = await _httpClient.GetAsync("Post");
        response.EnsureSuccessStatusCode();
        posts = await response.Content.ReadFromJsonAsync<List<UtterlyPost>>() ?? new List<UtterlyPost>();

        foreach (var post in posts)
        {
            await ConnectUserToPost(post);
        }

        return posts;
    }

    public async Task<bool> CreatePostAsync(UtterlyPost post)
    {
        var response = await _httpClient.PostAsync("Post", post.ToContent());
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdatePostAsync(UtterlyPost post)
    {
        var response = await _httpClient.PutAsJsonAsync($"Post/{post.Id}", post.ToContent());
        return response.IsSuccessStatusCode;
    }

    public async Task<List<UtterlyPost>> GetPostsByUserIdAsync(string id)
    {
        var response = await _httpClient.GetAsync($"Post/User/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return new List<UtterlyPost>();
        }
        return await response.Content.ReadFromJsonAsync<List<UtterlyPost>>() ?? new List<UtterlyPost>();
    }

    public async Task<List<UtterlyPost>> GetPostsByThreadIdAsync(int threadId)
    {
        var response = await _httpClient.GetAsync($"Post/Thread/{threadId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to fetch posts for the specified thread.");
        }
        var posts = await response.Content.ReadFromJsonAsync<List<UtterlyPost>>();

        foreach (var post in posts)
        {
            await ConnectUserToPost(post);
        }
        return posts;
    }
    public async Task<bool> DeletePost(int id)
    {
        var response = await _httpClient.DeleteAsync($"Post/{id}");
        return response.IsSuccessStatusCode;
    }
    private async Task ConnectUserToPost(UtterlyPost post)
    {
        post.User = await _userManager.FindByIdAsync(post.UserId);
    }

    internal async Task<UtterlyPost> GetPostById(int postId)
    {
        var response = await _httpClient.GetAsync($"Post/{postId}");
        response.EnsureSuccessStatusCode();
        UtterlyPost post = new UtterlyPost();
        post = await response.Content.ReadFromJsonAsync<UtterlyPost>() ?? new UtterlyPost();

        await ConnectUserToPost(post);

        return post;
    }
}
