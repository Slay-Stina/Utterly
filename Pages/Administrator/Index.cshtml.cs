using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Utterly.Areas.Identity.Data;
using Utterly.DAL;

namespace Utterly.Pages.Administrator;

public class IndexModel : PageModel
{
    public readonly RoleManager<IdentityRole> _roleManager;
    public readonly UserManager<UtterlyUser> _userManager;
    public readonly APIManager _utterlyPostAPIManager;

    public List<UtterlyPost> UtterlyPosts { get; set; }
    public IndexModel(
        RoleManager<IdentityRole> roleManager,
        UserManager<UtterlyUser> userManager,
        APIManager utterlyPostAPIManager
        )
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _utterlyPostAPIManager = utterlyPostAPIManager;
    }
    public List<UtterlyUser> Users { get; set; }
    public List<IdentityRole> Roles { get; set; }

    [DisplayName("Rollnamn")]
    [BindProperty(SupportsGet = true)]
    public string RoleName { get; set; }
    public async Task OnGetAsync()
    {
        UtterlyPosts = await _utterlyPostAPIManager.GetPostsAsync();
        Roles = await _roleManager.Roles.ToListAsync();
        Users = await _userManager.Users.ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (RoleName != null)
        {
            await CreateRole(RoleName);
        }

        return RedirectToPage("./Index");
    }

    public async Task CreateRole(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            IdentityRole role = new IdentityRole(roleName);
            await _roleManager.CreateAsync(role);
        }
    }
    public async Task<IActionResult> OnPostDeleteRoleAsync(string roleName)
    {
        if (roleName != null)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToPage("./Index");
                }
            }
        }
        return Page();
    }
    public async Task<IActionResult> OnPostSaveRoleAsync(string roleName, string newRoleName)
    {
        if (roleName != null && newRoleName != null)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                role.Name = newRoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToPage("./Index");
                }
            }
        }
        return Page();
    }
}
