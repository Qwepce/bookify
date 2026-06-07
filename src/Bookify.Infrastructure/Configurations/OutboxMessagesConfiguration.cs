using System.Reflection;
using Bookify.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configurations;

internal sealed class OutboxMessagesConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure( EntityTypeBuilder<OutboxMessage> builder )
    {
        builder.ToTable( "outbox_messages" );

        builder.HasKey( message => message.Id );

        builder.Property( message => message.Content )
            .HasColumnType( "jsonb" );
    }
}