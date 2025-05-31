using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Utterly.Areas.Identity.Data;

// Add profile data for application users by adding properties to the UtterlyUser class
public class UtterlyUser : IdentityUser
{
    [PersonalData]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }
    [PersonalData]
    public string[] Name { get; set; } = new string[2];
}

