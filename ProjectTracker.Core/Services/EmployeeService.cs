using Microsoft.EntityFrameworkCore;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Department;
using ProjectTracker.Core.ViewModels.Employee;
using ProjectTracker.Infrastructure.Data.Common;
using ProjectTracker.Infrastructure.Data.Entities;
using ProjectTracker.Core.ViewModels.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTracker.Core.ViewModels.Ticket;

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
                    Id = e.Id,
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

        public async Task<IEnumerable<EmployeeIdNameViewModel>> GetAllIdAndNameAsync()
        {
            return await repo.AllReadonly<Employee>()
                .Where(e => e.IsActive)
                .Select(e => new EmployeeIdNameViewModel()
                {
                    Id = e.Id,
                    UserName = e.UserName
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetUserNamesAsync()
            => await this.repo.AllReadonly<Employee>()
                        .Where(e => e.IsActive)
                        .Select(e => e.UserName)
                        .ToListAsync();

        public async Task<EmployeeDetailsViewModel> GetEmployeeDetailsAsync(string id)
        {
            var employee = await repo.AllReadonly<Employee>()
                .Where(e => e.IsActive && e.Id == id)
                .Select(e => new EmployeeDetailsViewModel()
                {
                    Id = e.Id,
                    UserName = e.UserName,
                    Email = e.Email,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Department = new DepartmentIdNameViewModel()
                    {
                        Id = e.Department.Id,
                        Name = e.Department.Name
                    },
                    IsLeader = e.LeadedDepartmentId == null,
                    Projects = e.EmployeesProjects
                        .Where(ep => ep.IsActive)
                        .Select(ep => new ProjectIdNameViewModel()
                        {
                            Id = ep.Project.Id,
                            Name = ep.Project.Name
                        })
                        .ToList(),
                    SubmittedTickets = e.SubmittedTickets
                        .Where(t => t.IsActive)
                        .Select(t => new TicketViewModel()
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Project = new ProjectIdNameViewModel()
                            {
                                Id = t.Project.Id,
                                Name = t.Project.Name
                            },
                            Priority = t.Priority,
                            Status = t.Status,
                            Date = t.CreatedOn
                        })
                        .ToList(),
                    AssignedTickets = e.AssignedTickets
                        .Where(t => t.IsActive)
                        .Select(t => new TicketViewModel()
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Project = new ProjectIdNameViewModel()
                            {
                                Id = t.Project.Id,
                                Name = t.Project.Name
                            },
                            Priority = t.Priority,
                            Status = t.Status,
                            Date = t.CreatedOn
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return employee;
        }
    }
}
