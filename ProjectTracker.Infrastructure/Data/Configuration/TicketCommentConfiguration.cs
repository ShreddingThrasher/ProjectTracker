using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTracker.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Infrastructure.Data.Configuration
{
    internal class TicketCommentConfiguration : IEntityTypeConfiguration<TicketComment>
    {
        public void Configure(EntityTypeBuilder<TicketComment> builder)
        {
            builder.HasOne(c => c.Ticket)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TicketId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(tc => tc.IsActive)
                .HasDefaultValue(true);
        }
    }
}
