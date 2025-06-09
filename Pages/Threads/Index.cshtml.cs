using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Utterly.Areas.Identity.Data;
using Utterly.DAL;

namespace Utterly.Pages.Threads;

public class IndexModel : PageModel
{
    private readonly UtterlyContext _context;
    private readonly UserManager<UtterlyUser> _userManager;
    private readonly APIManager _apiManager;

    public IndexModel(
        UtterlyContext context,
        UserManager<UtterlyUser> userManager,
        APIManager apiManager)
    {
        _context = context;
        _userManager = userManager;
        _apiManager = apiManager;
    }

    public Category Category { get; set; }
    public List<UtterlyThread> Threads { get; set; }
    [BindProperty]
    public NewThreadModel NewThread { get; set; }

    public async Task<IActionResult> OnGetAsync(int categoryId)
    {
        Category = await _context.Categories.FindAsync(categoryId);
        if (Category == null)
        {
            return NotFound();
        }

        Threads = await _context.Threads
            .Where(t => t.CategoryId == categoryId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        foreach (var thread in Threads)
        {
            thread.User = await _userManager.FindByIdAsync(thread.UserId);
            thread.Posts = await _apiManager.GetPostsByThreadIdAsync(thread.Id);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int categoryId)
    {
        // Get the current user
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        // Create and save the new thread
        var thread = new UtterlyThread
        {
            Title = NewThread.Title,
            CreatedAt = DateTime.Now,
            CategoryId = categoryId,
            UserId = userId
        };
        _context.Threads.Add(thread);
        await _context.SaveChangesAsync();

        // Create the first post in the thread
        var post = new UtterlyPost
        {
            Content = NewThread.Content,
            CreatedAt = DateTime.Now,
            ThreadId = thread.Id,
            UserId = userId
        };
        _context.UtterlyPosts.Add(post);
        await _context.SaveChangesAsync();

        return RedirectToPage(new { categoryId });
    }
}

public class NewThreadModel
{
    public string Title { get; set; }
    public string Content { get; set; }
}