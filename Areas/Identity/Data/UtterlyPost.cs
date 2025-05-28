using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utterly.Areas.Identity.Data;

public class UtterlyPost
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    // Foreign key till användaren
    [Required]
    public string UserId { get; set; } = string.Empty;

    // Navigeringsegenskap till användaren
    [ForeignKey("UserId")]
    public UtterlyUser? User { get; set; }
}
