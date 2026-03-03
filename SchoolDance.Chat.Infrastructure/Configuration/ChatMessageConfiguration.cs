using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolDance.Chat.Domain;

namespace SchoolDance.Chat.Infrastructure.Configuration;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.ChatRoomId).IsRequired();
        builder.Property(m => m.SenderId).IsRequired();
        builder.Property(m => m.Content).IsRequired().HasMaxLength(4000);
        builder.Property(m => m.SentAtUtc).IsRequired();

        builder.HasIndex(m => m.ChatRoomId);
        builder.HasIndex(m => m.SenderId);
    }
}
