using Microsoft.AspNetCore.Identity;
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

            SeedAdmin(builder);
            SeedRoles(builder);
            SeedUserRoles(builder);

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

        private void SeedAdmin(ModelBuilder builder)
        {
            var admin = new Employee()
            {
                Id = "33f73add-bb37-4d27-bb48-5fe0e682cd04",
                UserName = AdminConstants.UserName,
                Email = AdminConstants.Email,
                FirstName = "Admin",
                LastName = "User",
                LockoutEnabled = false
            };

            PasswordHasher<Employee> passwordHasher = new PasswordHasher<Employee>();
            passwordHasher.HashPassword(admin, "Admin123!");

            builder.Entity<Employee>().HasData(admin);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                { 
                    Id = "366cddc3-8ffb-4b6c-9df7-aaf99d737444", 
                    Name = "Admin", 
                    ConcurrencyStamp = "a920fb3d-f6c6-47d7-bc99-dbd6cce8876e", 
                    NormalizedName = "ADMIN" 
                },
                new IdentityRole() 
                { Id = "c65a4ecb-89b0-4c14-8a90-4bafea94d642", 
                    Name = "DepartmentLead", 
                    ConcurrencyStamp = "80dc19d2-9924-40c3-bca2-822fa7f727d7", 
                    NormalizedName = "DEPARTMENTLEAD" 
                }
            );
        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() 
                { 
                    RoleId = "366cddc3-8ffb-4b6c-9df7-aaf99d737444", 
                    UserId = "33f73add-bb37-4d27-bb48-5fe0e682cd04"
                }
            );
        }
    }
}