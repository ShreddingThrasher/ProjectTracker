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


        /// <summary>
        /// Adds an Employee to a role.
        /// </summary>
        /// <param name="userName">Employee's UserName</param>
        /// <param name="roleName">RoleName</param>
        /// <returns>IdentiyResult</returns>
        /// <exception cref="NullReferenceException">Throws if employee or role doesn't exist</exception>
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


        /// <summary>
        /// Assigns an Employee to a Department
        /// </summary>
        /// <param name="employeeId">EmployeeId</param>
        /// <param name="departmentId">ProjectId</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">Throws if Employee or Department doesn't exist.</exception>
        /// <exception cref="ArgumentException">Throws if Employee is already in the given Department or a leader to another Department</exception>
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


        /// <summary>
        /// Assigns an Employee to a Project
        /// </summary>
        /// <param name="employeeId">EmployeeId</param>
        /// <param name="projectId">ProjectId</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">Throws if Employee or Project doesn't exist.</exception>
        /// <exception cref="ArgumentException">Thors if the Employee is already assigned to the given Project.</exception>
        public async Task AssignToProjectAsync(string employeeId, Guid projectId)
        {
            var employee = await repo.All<Employee>()
                .Where(e => e.IsActive)
                .Include(e => e.EmployeesProjects)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if(employee == null)
            {
                throw new NullReferenceException("Employee doesn't exist.");
            }

            var project = await repo.All<Project>()
                .Where(p => p.IsActive)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if(project == null)
            {
                throw new NullReferenceException("Project doesn't exist.");
            }

            if(employee.EmployeesProjects.Any(ep => ep.ProjectId == projectId))
            {
                throw new ArgumentException("Employee is already assigned to this Project.");
            }

            var employeeProject = new EmployeeProject()
            {
                EmployeeId = employee.Id,
                Employee = employee,
                ProjectId = project.Id,
                Project = project,
                IsActive = true
            };

            await repo.AddAsync(employeeProject);
            await repo.SaveChangesAsync();
        }


        /// <summary>
        /// Creates new IdentityRole
        /// </summary>
        /// <param name="model"></param>
        /// <returns>IdentityResult</returns>
        public async Task<IdentityResult> CreateRoleAsync(CreateRoleViewModel model)
        {
            var role = new IdentityRole()
            {
                Name = model.RoleName
            };

            return await roleManager.CreateAsync(role);
        }


        /// <summary>
        /// Gets all Role Names
        /// </summary>
        /// <returns>List<string> containing all Role Names</returns>
        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            return await roleManager.Roles
                .Select(r => r.Name)
                .ToListAsync();
        }
    }
}
