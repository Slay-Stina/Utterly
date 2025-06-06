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
    private readonly APIManager _utterlyPostAPIManager;
    public List<UtterlyPost> UtterlyPosts;

    [BindProperty]
    public UtterlyPost UPost { get; set; }

    public IndexModel(
        UtterlyContext utterlyContext,
        UserManager<UtterlyUser> userManager,
        APIManager utterlyPostAPIManager)
    {
        _utterlyContext = utterlyContext;
        _userManager = userManager;
        _utterlyPostAPIManager = utterlyPostAPIManager;
    }

    public async Task OnGetAsync(int replyId)
    {
        UtterlyPosts = await _utterlyPostAPIManager.GetUtterlyPostsAsync();

        if (replyId != 0)
        {
            UPost = new();
            UPost.ParentPostId = replyId;
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            UtterlyPosts = await _utterlyPostAPIManager.GetUtterlyPostsAsync();
            return Page();
        }

        UPost.CreatedAt = DateTime.Now;
        UPost.UserId = _userManager.GetUserId(User);

        var success = await _utterlyPostAPIManager.CreateUtterlyPostAsync(UPost);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, "Kunde inte skapa inlägg via API.");
            UtterlyPosts = await _utterlyPostAPIManager.GetUtterlyPostsAsync();
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
