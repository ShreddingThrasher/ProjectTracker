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
                Database.Migrate();
            }
            else
            {
                Database.EnsureDeleted();
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
            SeedDepartment(builder);

            base.OnModelCreating(builder);
        }

        private void SeedAdmin(ModelBuilder builder)
        {
            var admin = new Employee()
            {
                Id = AdminConstants.AdminId,
                UserName = AdminConstants.UserName,
                Email = AdminConstants.Email,
                FirstName = "Administrator",
                LastName = "Administrator",
                NormalizedUserName = "ADMINISTRATOR",
                NormalizedEmail = "ADMINISTRATOR@MAIL.COM",
                LeadedDepartmentId = new Guid(AdminConstants.DepartmentId),
                LockoutEnabled = false
            };

            PasswordHasher<Employee> passwordHasher = new PasswordHasher<Employee>();
            admin.PasswordHash = passwordHasher.HashPassword(admin, "Administrator123!");

            builder.Entity<Employee>().HasData(admin);
        }

        private void SeedDepartment(ModelBuilder builder)
        {
            var department = new Department()
            {
                Id = new Guid("76c836e9-f620-4b7e-90e5-b8f15f1564a8"),
                Name = "Initial Department",
                LeadId = AdminConstants.AdminId,
                IsActive = true
            };

            builder.Entity<Department>().HasData(department);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                { 
                    Id = "366cddc3-8ffb-4b6c-9df7-aaf99d737444", 
                    Name = "Admin", 
                    NormalizedName = "ADMIN" 
                },
                new IdentityRole() 
                { 
                    Id = "c65a4ecb-89b0-4c14-8a90-4bafea94d642", 
                    Name = "DepartmentLead", 
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