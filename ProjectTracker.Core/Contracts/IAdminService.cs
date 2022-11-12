using Microsoft.AspNetCore.Identity;
using ProjectTracker.Core.ViewModels.Admin;
using ProjectTracker.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Contracts
{
    public interface IAdminService
    {
        Task<IEnumerable<string>> GetAllRolesAsync();

        Task<IdentityResult> CreateRoleAsync(CreateRoleViewModel model);

        Task<IdentityResult> AddToRoleAsync(string userName, string roleName);

        Task AssignToProjectAsync(string employeeId, Guid projectId);

        Task AssignToDepartmentAsync(string employeeId, Guid departmentId);
    }
}
