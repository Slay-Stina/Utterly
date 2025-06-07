using Microsoft.AspNetCore.Mvc.RazorPages;
using Utterly.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Utterly.Pages.Administrator;

public class UsersModel : PageModel
{
    private readonly UserManager<UtterlyUser> _userManager;

    public UsersModel(UserManager<UtterlyUser> userManager)
    {
        _userManager = userManager;
    }

    public List<UtterlyUser> Users { get; set; }

    public void OnGet()
    {
        Users = _userManager.Users.ToList();
    }

    public async Task<IActionResult> OnPostDeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }
        return RedirectToPage();
    }
}