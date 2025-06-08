using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Utterly.Areas.Identity.Data;

public class UtterlyUser : IdentityUser
{
    [PersonalData]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }

    // Navigation property for posts
    public ICollection<UtterlyPost> Posts { get; set; }

    // Profile picture as byte array
    [PersonalData]
    public byte[]? ProfilePicture { get; set; }
}

