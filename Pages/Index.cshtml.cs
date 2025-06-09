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
        UtterlyPosts = await _APIManager.GetPostsAsync();
        foreach(var post in UtterlyPosts)
        {
            post.User = await _userManager.FindByIdAsync(post.UserId);
            post.Thread = await _utterlyContext.Threads.FindAsync(post.ThreadId);
        }
    }
}
