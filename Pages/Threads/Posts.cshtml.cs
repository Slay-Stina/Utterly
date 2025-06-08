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
    private readonly UtterlyContext _context;
    public UtterlyThread UtterlyThread { get; set; }
    public List<UtterlyPost> Posts { get; set; }

    [BindProperty]
    public UtterlyPost NewPost { get; set; }
    public PostsModel(
        APIManager apiManager,
        UserManager<UtterlyUser> userManager,
        UtterlyContext utterlyContext)
    {
        _apiManager = apiManager;
        _userManager = userManager;
        _context = utterlyContext;
    }
    public async Task OnGetAsync(int threadId, int replyId)
    {
        UtterlyThread = _context.Threads.FirstOrDefault(t => t.Id == threadId);

        // Hämta inlägg för tråden
        Posts = await _apiManager.GetPostsByThreadIdAsync(threadId);

        // Ladda användare för varje inlägg
        foreach (var post in Posts)
        {
            post.User = await _userManager.FindByIdAsync(post.UserId);
            // Ingen extra kod behövs här för profilbild
        }

        if (replyId != 0)
        {
            NewPost = new UtterlyPost
            {
                ParentPostId = replyId,
                ThreadId = threadId
            };
        }
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Posts = await _apiManager.GetUtterlyPostsAsync();
            return Page();
        }

        NewPost.CreatedAt = DateTime.Now;
        NewPost.UserId = _userManager.GetUserId(User);

        var success = await _apiManager.CreateUtterlyPostAsync(NewPost);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, "Kunde inte skapa inlägg via API.");
            Posts = await _apiManager.GetUtterlyPostsAsync();
            return Page();
        }

        return RedirectToPage();
    }
}
