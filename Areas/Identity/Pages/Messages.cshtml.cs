using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Utterly.Areas.Identity.Data;

namespace Utterly.Areas.Identity.Pages;

public class MessagesModel : PageModel
{
    private readonly UserManager<UtterlyUser> _userManager;
    private readonly UtterlyContext _context;

    public MessagesModel(UserManager<UtterlyUser> userManager, UtterlyContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public UtterlyUser RecipientUser { get; set; }

    public List<UtterlyUser> ConversationPartners { get; set; } = new();

    public List<MessageViewModel> Messages { get; set; } = new();

    public class InputModel
    {
        public string RecipientId { get; set; }
        public string MessageText { get; set; }
    }

    public class MessageViewModel
    {
        public string SenderUserName { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public string SenderId { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(string userId)
    {
        var currentUserId = _userManager.GetUserId(User);

        // Hämta alla användar-id du har konversationer med
        var partnerIds = await _context.PersonalMessages
            .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
            .Select(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
            .Distinct()
            .ToListAsync();

        ConversationPartners = await _userManager.Users
            .Where(u => partnerIds.Contains(u.Id))
            .ToListAsync();

        if (!string.IsNullOrEmpty(userId))
        {
            RecipientUser = await _userManager.FindByIdAsync(userId);
            Input = new InputModel { RecipientId = userId };

            var messages = await _context.PersonalMessages
                .Where(m =>
                    (m.SenderId == currentUserId && m.ReceiverId == userId) ||
                    (m.SenderId == userId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            // Hämta alla unika avsändar-id
            var senderIds = messages.Select(m => m.SenderId).Distinct().ToList();
            var senders = await _userManager.Users
                .Where(u => senderIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.UserName);

            Messages = messages.Select(m => new MessageViewModel
            {
                SenderUserName = senders.TryGetValue(m.SenderId, out var name) ? name : "Okänd",
                Content = m.Content,
                SentAt = m.SentAt,
                SenderId = m.SenderId
            }).ToList();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var senderId = _userManager.GetUserId(User);
        if (!string.IsNullOrEmpty(Input.RecipientId) && !string.IsNullOrEmpty(Input.MessageText))
        {
            var message = new PersonalMessage
            {
                SenderId = senderId,
                ReceiverId = Input.RecipientId,
                Content = Input.MessageText,
                SentAt = DateTime.UtcNow
            };
            _context.PersonalMessages.Add(message);
            await _context.SaveChangesAsync();
            // Redirect to conversation or confirmation
            return RedirectToPage();
        }
        return Page();
    }
}
