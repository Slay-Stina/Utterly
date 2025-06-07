using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utterly.Areas.Identity.Data;
using Utterly.DAL;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Utterly.Pages.Administrator;

public class CategoriesModel : PageModel
{
    private readonly UtterlyContext _context;

    public CategoriesModel(UtterlyContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Category NewCategory { get; set; }

    public List<Category> Categories { get; set; }

    public void OnGet()
    {
        Categories = _context.Categories.ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Categories = _context.Categories.ToList();
            return Page();
        }

        _context.Categories.Add(NewCategory);
        await _context.SaveChangesAsync();
        return RedirectToPage();
    }
}