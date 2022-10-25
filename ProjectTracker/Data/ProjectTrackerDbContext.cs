using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Constants;
using ProjectTracker.Data.Entities;

namespace ProjectTracker.Data
{
    public class ProjectTrackerDbContext : IdentityDbContext<Employee>
    {
        public ProjectTrackerDbContext(DbContextOptions<ProjectTrackerDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EmployeeProject>()
                .HasKey(ep => new { ep.EmployeeId, ep.ProjectId });

            builder.Entity<EmployeeProject>()
                .HasOne(ep => ep.Project)
                .WithMany(p => p.AssignedEmployees)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<EmployeeProject>()
                .HasOne(ep => ep.Employee)
                .WithMany(e => e.EmployeesProjects)
                .HasForeignKey(ep => ep.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Employee>()
                .Property(e => e.UserName)
                .HasMaxLength(EmployeeConstants.UserNameMaxLength);

            builder.Entity<Employee>()
                .Property(e => e.Email)
                .HasMaxLength(EmployeeConstants.EmailMaxLength);

            base.OnModelCreating(builder);
        }
    }
}