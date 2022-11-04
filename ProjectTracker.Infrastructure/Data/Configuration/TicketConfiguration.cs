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
    internal class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasOne(t => t.Department)
                .WithMany(d => d.Tickets)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true);
        }
    }
}
