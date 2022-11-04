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
    internal class EmployeeProjectConfiguration : IEntityTypeConfiguration<EmployeeProject>
    {
        public void Configure(EntityTypeBuilder<EmployeeProject> builder)
        {
            builder.HasKey(ep => new { ep.EmployeeId, ep.ProjectId });

            builder.HasOne(ep => ep.Employee)
                .WithMany(e => e.EmployeesProjects)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ep => ep.Project)
                .WithMany(p => p.AssignedEmployees)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(ep => ep.IsActive)
                .HasDefaultValue(true);
        }
    }
}
