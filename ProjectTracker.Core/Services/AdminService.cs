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
                throw new NullReferenceException();
            }

            return await userManager.AddToRoleAsync(employee, roleName);
        }

        public async Task<IdentityResult> CreateRoleAsync(CreateRoleViewModel model)
        {
            var role = new IdentityRole()
            {
                Name = model.RoleName
            };

            return await roleManager.CreateAsync(role);
        }

        public async Task<IEnumerable<string>> GetAllRoles()
        {
            return await roleManager.Roles
                .Select(r => r.Name)
                .ToListAsync();
        }
    }
}
