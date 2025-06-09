using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utterly.Areas.Identity.Data;

public class UtterlyPost
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required]
    [DisplayName("Innehåll")]
    [StringLength(500, ErrorMessage = "Max 500 tecken", MinimumLength = 1)]
    public string Content { get; set; }
    public int? ParentPostId { get; set; }

    // Foreign key to UtterlyUser
    [Required]
    public string UserId { get; set; }

    // Navigation property
    public UtterlyUser? User { get; set; }

    // Foreign key till Thread
    [Required]
    public int ThreadId { get; set; }
    public UtterlyThread? Thread { get; set; }
}
