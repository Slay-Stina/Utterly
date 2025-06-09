using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utterly.Areas.Identity.Data;
using Utterly.DAL;

namespace Utterly.Pages.Threads;

public class PostsModel : PageModel
{
    private readonly APIManager _apiManager;
    private readonly UtterlyContext _context;
    public UtterlyThread UtterlyThread { get; set; }
    public List<UtterlyPost> Posts { get; set; }

    [BindProperty]
    public UtterlyPost NewPost { get; set; }
    public PostsModel(
        APIManager apiManager,
        UtterlyContext utterlyContext)
    {
        _apiManager = apiManager;
        _context = utterlyContext;
    }
    public async Task OnGetAsync(int threadId, int replyId, int deleteId)
    {
        UtterlyThread = _context.Threads.FirstOrDefault(t => t.Id == threadId);

        if (deleteId != 0)
        {
            var success = await _apiManager.DeletePost(deleteId);
            ModelState.AddModelError(string.Empty, "Inlägget har tagits bort.");
        }

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
            bool successfull;
            if (NewPost.Id == 0)
            {
                NewPost.CreatedAt = DateTime.UtcNow;
                successfull = await _apiManager.CreatePostAsync(NewPost);
            }
            else
            {
                successfull = await _apiManager.UpdatePostAsync(NewPost);
            }

            if (!successfull)
            {
                ModelState.AddModelError(string.Empty, "Kunde inte spara inlägget via API.");
                await OnGetAsync(NewPost.ThreadId,0,0);
                return Page();
            }

            return RedirectToPage(new { threadId = NewPost.ThreadId });
        }

        await OnGetAsync(NewPost.ThreadId, 0, 0);
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int postId, int threadId)
    {
        var success = await _apiManager.DeletePost(postId);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, "Kunde inte ta bort inlägget via API.");
            await OnGetAsync(threadId, 0, 0);
            return Page();
        }

        ModelState.AddModelError(string.Empty, "Inlägget har tagits bort.");
        await OnGetAsync(threadId, 0, 0);
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteOrSoftDeleteAsync(int postId, int threadId)
    {
        // Hämta inlägget
        var post = await _apiManager.GetPostById(postId);
        if (post == null)
        {
            ModelState.AddModelError(string.Empty, "Inlägget hittades inte.");
            await OnGetAsync(threadId, 0, 0);
            return Page();
        }

        // Kolla om det finns svar på inlägget
        bool hasReplies = _context.UtterlyPosts.Any(p => p.ParentPostId == postId);

        if (hasReplies)
        {
            post.Content = "Inlägget har tagits bort";
            post.UserId = "deleted";
            post.Thread = null;
            post.User = null;
            var success = await _apiManager.UpdatePostAsync(post);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Kunde inte soft-deleta inlägget via API.");
                await OnGetAsync(threadId, 0, 0);
                return Page();
            }
            ModelState.AddModelError(string.Empty, "Inlägget har tagits bort.");
        }
        else
        {
            var success = await _apiManager.DeletePost(postId);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Kunde inte ta bort inlägget via API.");
                await OnGetAsync(threadId, 0, 0);
                return Page();
            }
            ModelState.AddModelError(string.Empty, "Inlägget har tagits bort.");
        }

        await OnGetAsync(threadId, 0, 0);
        return Page();
    }
}
