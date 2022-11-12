using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Admin;
using ProjectTracker.Infrastructure.Data.Common;
using ProjectTracker.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IRepository repo;
        private readonly UserManager<Employee> userManager;

        public AdminService(
            RoleManager<IdentityRole> _roleManager,
            IRepository _repo,
            UserManager<Employee> _userManager)
        {
            roleManager = _roleManager;
            repo = _repo;
            userManager = _userManager;
        }

        public async Task<IdentityResult> AddToRoleAsync(string userName, string roleName)
        {
            var employee = await userManager.FindByNameAsync(userName);

            if(employee == null)
            {
                throw new NullReferenceException("Employee doesn't exist!");
            }

            if(!roleManager.Roles.Select(r => r.Name).Contains(roleName))
            {
                throw new NullReferenceException("Role doesn't exist!");
            }

            return await userManager.AddToRoleAsync(employee, roleName);
        }

        public async Task AssignToDepartmentAsync(string employeeId, Guid departmentId)
        {
            var employee = await repo.All<Employee>()
                .Where(e => e.IsActive)
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if(employee == null)
            {
                throw new NullReferenceException("Employee doesn't exist.");
            }

            var department = await repo.All<Department>()
                .Where(d => d.IsActive)
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == departmentId);

            if(department == null)
            {
                throw new NullReferenceException("Department doesn't exist.");
            }

            if(employee?.Department?.Id == departmentId)
            {
                throw new ArgumentException("The Employee is already in this Department.");
            }

            if(employee.LeadedDepartmentId != null)
            {
                throw new ArgumentException("This Employee is a leader of Department and cannot be assigned.");
            }

            employee.DepartmentId = department.Id;

            await repo.SaveChangesAsync();
        }

        public async Task AssignToProjectAsync(string employeeId, Guid projectId)
        {
            var employee = await repo.All<Employee>()
                .Where(e => e.IsActive)
                .Include(e => e.EmployeesProjects)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if(employee == null)
            {
                throw new NullReferenceException();
            }

            var project = await repo.All<Project>()
                .Where(p => p.IsActive)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if(project == null)
            {
                throw new NullReferenceException();
            }

            if(employee.EmployeesProjects.Any(ep => ep.ProjectId == projectId))
            {
                throw new ArgumentException();
            }

            var employeeProject = new EmployeeProject()
            {
                EmployeeId = employee.Id,
                Employee = employee,
                ProjectId = project.Id,
                Project = project
            };

            await repo.AddAsync(employeeProject);
            await repo.SaveChangesAsync();
        }

        public async Task<IdentityResult> CreateRoleAsync(CreateRoleViewModel model)
        {
            var role = new IdentityRole()
            {
                Name = model.RoleName
            };

            return await roleManager.CreateAsync(role);
        }

        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            return await roleManager.Roles
                .Select(r => r.Name)
                .ToListAsync();
        }
    }
}
