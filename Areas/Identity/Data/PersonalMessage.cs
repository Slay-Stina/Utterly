namespace Utterly.Areas.Identity.Data;

public class PersonalMessage
{
    public int Id { get; set; }
    public string SenderId { get; set; } // Foreign key to UtterlyUser
    public string ReceiverId { get; set; } // Foreign key to UtterlyUser
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    // Navigation properties
    public UtterlyUser Sender { get; set; }
    public UtterlyUser Receiver { get; set; }
}
