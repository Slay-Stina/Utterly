using System.ComponentModel.DataAnnotations;
using Utterly.Areas.Identity.Data;

public class UtterlyThread
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    public DateTime CreatedAt { get; set; }

    // Foreign key to Category
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    // Foreign key to User
    public string UserId { get; set; }
    public UtterlyUser User { get; set; }

    // Navigation property
    public ICollection<UtterlyPost>? Posts { get; set; }
}