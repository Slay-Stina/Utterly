using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utterly.Areas.Identity.Data;
using Utterly.DAL;

namespace Utterly.Pages.Threads;

public class PostsModel : PageModel
{
    private readonly APIManager _apiManager;
    private readonly UserManager<UtterlyUser> _userManager;
    public List<UtterlyPost> Posts { get; set; }

    [BindProperty]
    public UtterlyPost UPost { get; set; }
    public PostsModel(APIManager apiManager, UserManager<UtterlyUser> userManager)
    {
        _apiManager = apiManager;
        _userManager = userManager;
    }
    public async Task OnGetAsync(int threadId)
    {
        // Fetch posts for the specified thread ID
        Posts = await _apiManager.GetPostsByThreadIdAsync(threadId);

        // Ensure each post's User property is loaded
        foreach (var post in Posts)
        {
            if (post.User == null && !string.IsNullOrEmpty(post.UserId))
            {
                post.User = await _userManager.FindByIdAsync(post.UserId);
            }
        }
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Posts = await _apiManager.GetUtterlyPostsAsync();
            return Page();
        }

        UPost.CreatedAt = DateTime.Now;
        UPost.UserId = _userManager.GetUserId(User);

        var success = await _apiManager.CreateUtterlyPostAsync(UPost);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, "Kunde inte skapa inlägg via API.");
            Posts = await _apiManager.GetUtterlyPostsAsync();
            return Page();
        }

        return RedirectToPage();
    }
}
