using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Utterly.Pages.Administrator;

public class RolesModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesModel(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public List<IdentityRole> Roles { get; set; }

    [BindProperty]
    public string RoleName { get; set; }

    public void OnGet()
    {
        Roles = _roleManager.Roles.ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!string.IsNullOrWhiteSpace(RoleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(RoleName));
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteRoleAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role != null)
        {
            await _roleManager.DeleteAsync(role);
        }
        return RedirectToPage();
    }
}