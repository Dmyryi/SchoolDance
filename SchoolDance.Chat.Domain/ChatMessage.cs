namespace SchoolDance.Chat.Domain;

public class ChatMessage
{
    public Guid Id { get; set; }

    public Guid ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; } = null!;

    // UserId из основного сервиса (MySQL)
    public Guid SenderId { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTime SentAtUtc { get; set; } = DateTime.UtcNow;

    public bool IsRead { get; set; }
}