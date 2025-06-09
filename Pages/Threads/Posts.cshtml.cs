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
        if (ModelState.IsValid)
        {
            NewPost.CreatedAt = DateTime.UtcNow;

            var success = await _apiManager.CreateUtterlyPostAsync(NewPost);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Kunde inte skapa inlägg via API.");
            }
        }
        // Pass threadId (and replyId if needed) to the page
        return RedirectToPage(new { threadId = NewPost.ThreadId });
    }
}
