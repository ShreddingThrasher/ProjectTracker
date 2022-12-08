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
            if (Database.IsRelational())
            {

            }
            else
            {
                Database.EnsureCreated();
            }
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

            base.OnModelCreating(builder);
        }

        private void SeedAdmin(ModelBuilder builder)
        {
            var admin = new Employee()
            {
                Id = AdminConstants.AdminId,
                UserName = AdminConstants.UserName,
                Email = AdminConstants.Email,
                FirstName = "Admin",
                LastName = "User",
                LockoutEnabled = false
            };

            PasswordHasher<Employee> passwordHasher = new PasswordHasher<Employee>();
            admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin123!");

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
                    RoleId = AdminConstants.RoleId, 
                    UserId = AdminConstants.AdminId
                }
            );
        }

    }
}