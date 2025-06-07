using System.ComponentModel.DataAnnotations;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    // Navigation property
    public ICollection<UtterlyThread>? Threads { get; set; }
}