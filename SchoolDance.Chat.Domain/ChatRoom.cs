namespace SchoolDance.Chat.Domain;

public class ChatRoom
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid TrainerId { get; set; }

    public Guid? SubscriptionId { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}