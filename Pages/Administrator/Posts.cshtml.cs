using Microsoft.AspNetCore.Mvc.RazorPages;
using Utterly.Areas.Identity.Data;
using Utterly.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Utterly.Pages.Administrator;

public class PostsModel : PageModel
{
    private readonly UtterlyContext _context;

    public PostsModel(UtterlyContext context)
    {
        _context = context;
    }

    public List<UtterlyPost> UtterlyPosts { get; set; }

    public void OnGet()
    {
        UtterlyPosts = _context.UtterlyPosts.Include(p => p.User).ToList();
    }

    public async Task<IActionResult> OnPostDeletePostAsync(int id)
    {
        var post = await _context.UtterlyPosts.FindAsync(id);
        if (post != null)
        {
            _context.UtterlyPosts.Remove(post);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
    }
}