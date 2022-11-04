using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Infrastructure.Data.Configuration;
using ProjectTracker.Infrastructure.Data.Entities;
using ProjectTracker.Infrastructure.DataConstants;

namespace ProjectTracker.Infrastructure.Data
{
    public class ProjectTrackerDbContext : IdentityDbContext<Employee>
    {
        public ProjectTrackerDbContext(DbContextOptions<ProjectTrackerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketComment> Comments { get; set; }

        public DbSet<TicketChange> Changes { get; set; }

        public DbSet<EmployeeProject> EmployeesProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new EmployeeConfiguration());
            builder.ApplyConfiguration(new DepartmentConfiguration());
            builder.ApplyConfiguration(new EmployeeProjectConfiguration());
            builder.ApplyConfiguration(new ProjectConfiguration());
            builder.ApplyConfiguration(new TicketConfiguration());
            builder.ApplyConfiguration(new TicketChangeConfiguration());
            builder.ApplyConfiguration(new TicketCommentConfiguration());

            //builder.Entity<EmployeeProject>()
            //    .HasKey(ep => new { ep.EmployeeId, ep.ProjectId });


            //Delete behaviour
            //builder.Entity<EmployeeProject>()
            //    .HasOne(ep => ep.Employee)
            //    .WithMany(e => e.EmployeesProjects)
            //    .HasForeignKey(e => e.EmployeeId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<EmployeeProject>()
            //    .HasOne(ep => ep.Project)
            //    .WithMany(p => p.AssignedEmployees)
            //    .HasForeignKey(p => p.ProjectId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Employee>()
            //    .Property(e => e.LeadedDepartmentId)
            //    .IsRequired(false);

            //builder.Entity<Employee>()
            //    .HasOne(e => e.Department)
            //    .WithMany(d => d.Employees)
            //    .HasForeignKey(e => e.DepartmentId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Employee>()
            //    .HasOne(e => e.LeadedDepartment)
            //    .WithOne(d => d.Lead)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Ticket>()
            //    .HasOne(t => t.Department)
            //    .WithMany(d => d.Tickets)
            //    .HasForeignKey(t => t.DepartmentId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<TicketComment>()
            //    .HasOne(c => c.Ticket)
            //    .WithMany(t => t.Comments)
            //    .HasForeignKey(c => c.TicketId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //Employee Foreign keys
            //builder.Entity<Employee>()
            //    .Property(e => e.LeadedDepartmentId)
            //    .IsRequired(false);

            //builder.Entity<Employee>()
            //    .Property(e => e.DepartmentId)
            //    .IsRequired(false);


            //ASP.NET Identity User properties max length
            //builder.Entity<Employee>()
            //    .Property(e => e.UserName)
            //    .HasMaxLength(EmployeeConstants.UserNameMaxLength);

            //builder.Entity<Employee>()
            //    .Property(e => e.Email)
            //    .HasMaxLength(EmployeeConstants.EmailMaxLength);


            //IsActive default value for all entities
            //builder.Entity<TicketChange>()
            //    .Property(c => c.IsActive)
            //    .HasDefaultValue(true);

            //builder.Entity<TicketComment>()
            //    .Property(c => c.IsActive)
            //    .HasDefaultValue(true);

            //builder.Entity<Department>()
            //    .Property(c => c.IsActive)
            //    .HasDefaultValue(true);

            //builder.Entity<Employee>()
            //    .Property(c => c.IsActive)
            //    .HasDefaultValue(true);

            //builder.Entity<EmployeeProject>()
            //    .Property(c => c.IsActive)
            //    .HasDefaultValue(true);

            //builder.Entity<Project>()
            //    .Property(c => c.IsActive)
            //    .HasDefaultValue(true);

            //builder.Entity<Ticket>()
            //    .Property(c => c.IsActive)
            //    .HasDefaultValue(true);

            base.OnModelCreating(builder);
        }
    }
}