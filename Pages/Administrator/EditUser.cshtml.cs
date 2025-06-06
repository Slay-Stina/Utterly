using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Utterly.Areas.Identity.Data;
using Utterly.DAL;

namespace Utterly.Pages.Administrator;

public class EditUserModel : PageModel
{
    private readonly UserManager<UtterlyUser> _userManager;
    private readonly APIManager _apiManager;

    public EditUserModel(UserManager<UtterlyUser> userManager, APIManager apiManager)
    {
        _userManager = userManager;
        _apiManager = apiManager;
    }

    [BindProperty]
    public EditUserInputModel Input { get; set; }

    public UtterlyUser User { get; set; }
    public List<UtterlyPost> UserPosts { get; set; } = new();
    public bool IsEditMode { get; set; }

    public class EditUserInputModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Användarnamn")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "E-post")]
        public string Email { get; set; }

        [Display(Name = "Födelsedatum")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(string id, bool edit = false)
    {
        if (id == null) return NotFound();

        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        User = user;
        IsEditMode = edit;

        if (edit)
        {
            Input = new EditUserInputModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                BirthDate = user.BirthDate
            };
        }

        UserPosts = await _apiManager.GetUtterlyPostsByUserIdAsync(user.Id);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        IsEditMode = true; // Visa formuläret om validering misslyckas

        if (!ModelState.IsValid) return Page();

        var user = await _userManager.FindByIdAsync(Input.Id);
        if (user == null) return NotFound();

        user.UserName = Input.UserName;
        user.Email = Input.Email;
        user.BirthDate = Input.BirthDate;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return Page();
        }

        return RedirectToPage(new { id = user.Id, edit = false });
    }
}