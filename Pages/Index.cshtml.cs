using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utterly.Areas.Identity.Data;
using Utterly.DAL;

namespace Utterly.Pages;

public class IndexModel : PageModel
{
    private readonly UtterlyContext _utterlyContext;
    private readonly UserManager<UtterlyUser> _userManager;
    private readonly APIManager _APIManager;
    public List<UtterlyPost> UtterlyPosts;

    [BindProperty]
    public UtterlyPost NewPost { get; set; }

    public IndexModel(
        UtterlyContext utterlyContext,
        UserManager<UtterlyUser> userManager,
        APIManager APIManager)
    {
        _utterlyContext = utterlyContext;
        _userManager = userManager;
        _APIManager = APIManager;
    }

    public async Task OnGetAsync(int replyId)
    {
        UtterlyPosts = await _APIManager.GetUtterlyPostsAsync();

        if (replyId != 0)
        {
            NewPost = new();
            NewPost.ParentPostId = replyId;
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            UtterlyPosts = await _APIManager.GetUtterlyPostsAsync();
            return Page();
        }

        NewPost.CreatedAt = DateTime.Now;
        NewPost.UserId = _userManager.GetUserId(User);

        var success = await _APIManager.CreateUtterlyPostAsync(NewPost);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, "Kunde inte skapa inlägg via API.");
            UtterlyPosts = await _APIManager.GetUtterlyPostsAsync();
            return Page();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> DeletePost(int id)
    {
        var post = await _utterlyContext.UtterlyPosts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        _utterlyContext.UtterlyPosts.Remove(post);
        await _utterlyContext.SaveChangesAsync();
        return RedirectToPage();
    }
}
