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

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Change> Changes { get; set; }

        public DbSet<EmployeeProject> EmployeesProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EmployeeProject>()
                .HasKey(ep => new { ep.EmployeeId, ep.ProjectId });

            builder.Entity<EmployeeProject>()
                .HasOne(ep => ep.Employee)
                .WithMany(e => e.EmployeesProjects)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<EmployeeProject>()
                .HasOne(ep => ep.Project)
                .WithMany(p => p.AssignedEmployees)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Employee>()
                .Property(e => e.LeadedDepartmentId)
                .IsRequired(false);

            builder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Employee>()
                .HasOne(e => e.LeadedDepartment)
                .WithOne(d => d.Lead)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ticket>()
                .HasOne(t => t.Department)
                .WithMany(d => d.Tickets)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasOne(c => c.Ticket)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TicketId)
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