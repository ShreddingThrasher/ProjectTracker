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
    internal class TicketChangeConfiguration : IEntityTypeConfiguration<TicketChange>
    {
        public void Configure(EntityTypeBuilder<TicketChange> builder)
        {
            builder.Property(c => c.IsActive)
                .HasDefaultValue(true);
        }
    }
}
