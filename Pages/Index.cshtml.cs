using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Utterly.Areas.Identity.Data;

namespace Utterly.Pages;

public class IndexModel : PageModel
{
    private readonly UtterlyContext _utterlyContext;
    private readonly UserManager<UtterlyUser> _userManager;
    public List<UtterlyPost> UtterlyPosts;

    [BindProperty]
    public UtterlyPost UPost { get; set; }

    public IndexModel(UtterlyContext utterlyContext, UserManager<UtterlyUser> userManager)
    {
        _utterlyContext = utterlyContext;
        _userManager = userManager;
    }

    public async Task OnGetAsync(int replyId)
    {
        UtterlyPosts = await _utterlyContext.UtterlyPosts
            .Include(p => p.User)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        if(replyId != 0)
        {
            UPost = new();
            UPost.ParentPostId = replyId;
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            UtterlyPosts = await _utterlyContext.UtterlyPosts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return Page();
        }

        UPost.CreatedAt = DateTime.Now;

        _utterlyContext.UtterlyPosts.Add(UPost);
        await _utterlyContext.SaveChangesAsync();

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
