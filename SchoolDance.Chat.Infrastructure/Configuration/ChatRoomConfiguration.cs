using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolDance.Chat.Domain;

namespace SchoolDance.Chat.Infrastructure.Configuration;

public class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
{
    public void Configure(EntityTypeBuilder<ChatRoom> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.StudentId).IsRequired();
        builder.Property(r => r.TrainerId).IsRequired();
        builder.Property(r => r.CreatedAtUtc).IsRequired();

        builder.HasIndex(r => r.SubscriptionId);
        builder.HasIndex(r => new { r.StudentId, r.TrainerId });

        builder
            .HasMany(r => r.Messages)
            .WithOne(m => m.ChatRoom)
            .HasForeignKey(m => m.ChatRoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
