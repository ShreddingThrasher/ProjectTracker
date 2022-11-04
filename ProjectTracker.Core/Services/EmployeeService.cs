using Microsoft.EntityFrameworkCore;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Employee;
using ProjectTracker.Infrastructure.Data.Common;
using ProjectTracker.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository repo;

        public EmployeeService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetAll()
        {
            return await repo.AllReadonly<Employee>()
                .Where(e => e.IsActive)
                .Select(e => new EmployeeViewModel()
                {
                    FullName = e.FirstName + " " + e.LastName,
                    Department = e.Department.Name,
                    AssignedProjects = e.EmployeesProjects.Count,
                    AssignedTickets = e.AssignedTickets.Count,
                    Email = e.Email
                })
                .ToListAsync();
        }

        public async Task<int> GetCount()
            => await this.repo.AllReadonly<Employee>().Where(e => e.IsActive).CountAsync();

        public async Task<IEnumerable<string>> GetUserNamesAsync()
            => await this.repo.AllReadonly<Employee>()
                        .Where(e => e.IsActive)
                        .Select(e => e.UserName)
                        .ToListAsync();
    }
}
