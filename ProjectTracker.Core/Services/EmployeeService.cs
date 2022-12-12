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
using ProjectTracker.Core.Constants;
using ProjectTracker.Infrastructure.Data.Entities.Enums;
using ProjectTracker.Infrastructure.DataConstants;

namespace ProjectTracker.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository repo;

        public EmployeeService(IRepository _repo)
        {
            repo = _repo;
        }


        /// <summary>
        /// Gets all active Employees
        /// </summary>
        /// <returns>Collection of EmployeeViewModel</returns>
        public async Task<IEnumerable<EmployeeViewModel>> GetAllAsync()
        {
            return await repo.AllReadonly<Employee>()
                .Where(e => e.IsActive)
                .Select(e => new EmployeeViewModel()
                {
                    Id = e.Id,
                    FullName = e.FirstName + " " + e.LastName,
                    Department = e.Department.Name,
                    AssignedProjects = e.EmployeesProjects
                        .Where(p => p.IsActive)
                        .Count(),
                    AssignedTickets = e.AssignedTickets
                        .Where(t => t.IsActive)
                        .Count(),
                    Email = e.Email
                })
                .ToListAsync();
        }


        /// <summary>
        /// Gets the count of all active Employees
        /// </summary>
        /// <returns>Count as int</returns>
        public async Task<int> GetCountAsync()
            => await this.repo.AllReadonly<Employee>().Where(e => e.IsActive).CountAsync();


        /// <summary>
        /// Gets Id and UserName for all current Employees
        /// </summary>
        /// <returns>Collection of EmployeeIdNameViewModel</returns>
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


        /// <summary>
        /// Gets the UserNames for all currentEmployees
        /// </summary>
        /// <returns>Collection of string</returns>
        public async Task<IEnumerable<string>> GetUserNamesAsync()
            => await this.repo.AllReadonly<Employee>()
                        .Where(e => e.IsActive)
                        .Select(e => e.UserName)
                        .ToListAsync();


        /// <summary>
        /// Gets details for a given Employee or null if the Employee doesn't exist.
        /// </summary>
        /// <param name="id">EmployeeId</param>
        /// <returns>Model holding detailed data for the Employee</returns>
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
                    Department = e.Department == null ? null : new DepartmentIdNameViewModel()
                    {
                        Id = e.Department.Id,
                        Name = e.Department.Name
                    },
                    IsLeader = e.LeadedDepartmentId != null,
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


        /// <summary>
        /// Gets all Employees that are not assigned to a Department
        /// </summary>
        /// <returns>Collection of EmployeeViewModel</returns>
        public async Task<IEnumerable<EmployeeViewModel>> GetUnassignedAsync()
        {
            return await repo.AllReadonly<Employee>()
                .Where(e => e.IsActive && e.DepartmentId == null)
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


        /// <summary>
        /// Gets all Employees that are assigned to a Department
        /// </summary>
        /// <returns>Collection of EmployeeViewModel</returns>
        public async Task<IEnumerable<EmployeeViewModel>> GetActiveAsync()
        {
            return await repo.AllReadonly<Employee>()
                .Where(e => e.IsActive && e.DepartmentId != null)
                .Select(e => new EmployeeViewModel()
                {
                    Id = e.Id,
                    Username = e.UserName,
                    FullName = e.FirstName + " " + e.LastName,
                    Department = e.Department.Name,
                    AssignedProjects = e.EmployeesProjects
                        .Where(p => p.IsActive)
                        .Count(),
                    AssignedTickets = e.AssignedTickets
                        .Where(t => t.IsActive)
                        .Count(),
                    Email = e.Email
                })
                .ToListAsync();
        }


        /// <summary>
        /// Sets an Employee to innactive
        /// </summary>
        /// <param name="employeeId">EmployeeId</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">Throws if the Employee doesn't exist</exception>
        public async Task RemoveById(string employeeId)
        {
            var employee = await repo.All<Employee>()
                .Where(e => e.IsActive && e.Id == employeeId)
                .Include(e => e.EmployeesProjects)
                .Include(e => e.LeadedDepartment)
                .Include(e => e.AssignedTickets)
                .Include(e => e.SubmittedTickets)
                .FirstOrDefaultAsync();

            if(employee == null)
            {
                throw new NullReferenceException();
            }

            employee.IsActive = false;

            if(employee.LeadedDepartmentId != null)
            {
                var admin = await repo.All<Employee>()
                    .Where(e => e.Email == AdminConstants.Email)
                    .FirstOrDefaultAsync();

                employee.LeadedDepartment.LeadId = admin.Id;
            }

            foreach (var project in employee.EmployeesProjects)
            {
                project.IsActive = false;
            }

            foreach (var ticket in employee.AssignedTickets)
            {
                ticket.AssignedEmployee = null;
                
                if(ticket.Status != Status.Done)
                {
                    ticket.Status = Status.Open;
                }
            }

            foreach (var ticket in employee.SubmittedTickets)   
            {
                ticket.IsActive = false;
            }

            await repo.SaveChangesAsync();
        }
    }
}
