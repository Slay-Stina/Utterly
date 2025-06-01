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
    public UtterlyPost Post { get; set; }

    public IndexModel(UtterlyContext utterlyContext, UserManager<UtterlyUser> userManager)
    {
        _utterlyContext = utterlyContext;
        _userManager = userManager;
    }

    public async Task OnGetAsync()
    {
        UtterlyPosts = await _utterlyContext.UtterlyPosts
            .Include(p => p.User)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
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

        Post.CreatedAt = DateTime.Now;

        _utterlyContext.UtterlyPosts.Add(Post);
        await _utterlyContext.SaveChangesAsync();

        return RedirectToPage();
    }
}
