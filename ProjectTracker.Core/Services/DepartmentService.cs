using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Department;
using ProjectTracker.Core.ViewModels.Employee;
using ProjectTracker.Core.ViewModels.Project;
using ProjectTracker.Core.ViewModels.Ticket;
using ProjectTracker.Infrastructure.Data.Common;
using ProjectTracker.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository repo;

        public DepartmentService(IRepository _repo)
        {
            repo = _repo;
        }

        /// <summary>
        /// Creates a new Department async
        /// </summary>
        /// <param name="model">View model containing the new Department data</param>
        /// <returns></returns>
        public async Task CreateAsync(CreateDepartmentViewModel model)
        {
            var lead = await repo.All<Employee>()
                .Where(e => e.IsActive)
                .FirstOrDefaultAsync(e => e.Id == model.LeadId);

            if(lead == null)
            {
                throw new NullReferenceException("The given Employee doesn't exist.");
            }

            if(lead.LeadedDepartmentId != null)
            {
                throw new ArgumentException("The given Employee is already Leader of another Department.");
            }

            var department = new Department()
            {
                Name = model.Name,
                LeadId = model.LeadId,
                Lead = lead
            };

            department.Employees.Add(lead);
            lead.Department = department;

            await repo.AddAsync(department);
            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Sets a given Department from Active to Inactive
        /// </summary>
        /// <param name="id">Department Id</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">Throws if the given Department doesn't exist.</exception>
        public async Task DeleteAsync(Guid id)
        {
            var department = await repo.All<Department>()
                .Where(d => d.IsActive && d.Id == id)
                .Include(d => d.Employees)
                .ThenInclude(e => e.EmployeesProjects)
                .ThenInclude(ep => ep.Project)
                .Include(d => d.Lead)
                .Include(d => d.Projects)
                .Include(d => d.Tickets)
                .FirstOrDefaultAsync();

            if(department == null)
            {
                throw new NullReferenceException();
            }

            department.IsActive = false;

            foreach (var employee in department.Employees)
            {
                employee.DepartmentId = null;

                foreach (var ep in employee.EmployeesProjects.Where(ep => ep.Project.DepartmentId == department.Id))
                {
                    ep.IsActive = false;
                }
            }

            foreach (var project in department.Projects)
            {
                project.IsActive = false;
            }

            foreach (var ticket in department.Tickets)
            {
                ticket.IsActive = false;
            }

            department.Lead.LeadedDepartmentId = null;

            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Edits existing Department
        /// </summary>
        /// <param name="model">Model with the Edit data</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">Throws if the given Department doesn't exist</exception>
        /// <exception cref="ArgumentException">Throws if the given Department Leader is already leader of another Department</exception>
        public async Task EditAsync(EditDepartmentViewModel model)
        {
            var department = await repo.All<Department>()
                .Where(d => d.IsActive && d.Id == model.Id)
                .Include(d => d.Lead)
                .FirstOrDefaultAsync();

            if(department == null)
            {
                throw new NullReferenceException();
            }

            var lead = await repo.All<Employee>()
                .Where(e => e.IsActive && e.Id == model.LeadId)
                .FirstOrDefaultAsync();

            if(lead == null)
            {
                throw new NullReferenceException();
            }
            
            if(lead.LeadedDepartmentId != null)
            {
                throw new ArgumentException("The Employee is already leader of another department");
            }

            department.Name = model.Name;
            department.LeadId = model.LeadId;

            await repo.SaveChangesAsync();
        }


        /// <summary>
        /// Gets all active departments in the database
        /// </summary>
        /// <returns>IEnumerable<DepartmentViewModel> departments</returns>
        public async Task<IEnumerable<DepartmentViewModel>> GetAllAsync()
        {
            return await repo.AllReadonly<Department>()
                .Where(d => d.IsActive)
                .Select(d => new DepartmentViewModel()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Employees = d.Employees
                        .Where(e => e.IsActive)
                        .Count(),
                    Projects = d.Projects
                        .Where(p => p.IsActive)
                        .Count(),
                    Tickets = d.Tickets
                        .Where(t => t.IsActive)
                        .Count(),
                    LeaderId = d.LeadId
                })
                .ToListAsync();
        }


        /// <summary>
        /// Gets Id and Name for all active departments in the database
        /// </summary>
        /// <returns>IEnumerable<DepartmentIdNameViewModel></returns>
        public async Task<IEnumerable<DepartmentIdNameViewModel>> GetAllIdAndNameAsync()
        {
            return await repo.AllReadonly<Department>()
                .Where(d => d.IsActive)
                .Select(d => new DepartmentIdNameViewModel()
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToListAsync();
        }


        /// <summary>
        /// Gets the count of all active departments in the database
        /// </summary>
        /// <returns>Count as int32</returns>
        public async Task<int> GetCountAsync()
            => await this.repo.AllReadonly<Department>().Where(d => d.IsActive).CountAsync();


        /// <summary>
        /// Gets details for an Active Department by Id async
        /// </summary>
        /// <param name="departmentId">Department Id</param>
        /// <returns>DepartmentDetailsViewModel or null</returns>
        public async Task<DepartmentDetailsViewModel> GetDepartmentDetailsAsync(Guid departmentId)
        {
#pragma warning disable CS8603 // Possible null reference return.
            var model =  await repo.AllReadonly<Department>()
                .Where(d => d.IsActive && d.Id == departmentId)
                .Include(d => d.Lead)
                .Include(d => d.Employees)
                .Include(d => d.Projects)
                .Include(d => d.Tickets)
                .ThenInclude(d => d.Project)
                .Include(d => d.Tickets)
                .ThenInclude(d => d.Submitter)
                .Select(d => new DepartmentDetailsViewModel()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Leader = new EmployeeViewModel()
                    {
                        Id = d.Lead.Id,
                        FullName = d.Lead.FirstName + " " + d.Lead.LastName,
                        Email = d.Lead.Email
                    },
                    Employees = d.Employees
                        .Where(e => e.IsActive)
                        .Select(e => new EmployeeViewModel()
                        {
                            Id = e.Id,
                            FullName = e.FirstName + " " + e.LastName,
                            Email = e.Email
                        })
                        .ToList(),
                    Projects = d.Projects
                        .Where(p => p.IsActive)
                        .Select(p => new ProjectViewModel()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description
                        })
                        .ToList(),
                    Tickets = d.Tickets
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
                            Submitter = new EmployeeIdNameViewModel()
                            {
                                Id = t.Submitter.Id,
                                UserName = t.Submitter.UserName
                            },
                            Date = t.CreatedOn,
                            Priority = t.Priority,
                            Status = t.Status
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return model;
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// Gets details for a Department to be edited.
        /// </summary>
        /// <param name="id">Department Id</param>
        /// <returns>EditDepartmentViewModelo</returns>
        /// <exception cref="NullReferenceException">Throws if there is no Department with the given Id.</exception>
        public async Task<EditDepartmentViewModel> GetEditDetailsAsync(Guid id)
        {
            var department = await repo.AllReadonly<Department>()
                .Where(d => d.IsActive && d.Id == id)
                .Select(d => new EditDepartmentViewModel()
                {
                    Id = d.Id,
                    Name = d.Name,
                    LeadId = d.LeadId,
                })
                .FirstOrDefaultAsync();

            if(department == null)
            {
                throw new NullReferenceException();
            }

            return department;
        }

        /// <summary>
        /// Gets all inactive departments in the database
        /// </summary>
        /// <returns>IEnumerable<DepartmentViewModel></returns>
        public async Task<IEnumerable<DepartmentViewModel>> GetInactiveDepartmentsAsync()
        {
            return await repo.AllReadonly<Department>()
                .Where(d => !d.IsActive)
                .Select(d => new DepartmentViewModel()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Employees = d.Employees.Count,
                    Projects = d.Projects.Count,
                    Tickets = d.Tickets.Count,
                })
                .ToListAsync();
        }


        /// <summary>
        /// Gets all Employees that can be set as Department Leader
        /// </summary>
        /// <returns>Id and Name for all Employees that are currently not a leader of another Department</returns>
        public async Task<IEnumerable<EmployeeIdNameViewModel>> GetPossibleLeadersAsync()
        {
            return await repo.AllReadonly<Employee>()
                .Where(e => e.IsActive && e.LeadedDepartmentId == null)
                .Select(e => new EmployeeIdNameViewModel()
                {
                    Id = e.Id,
                    UserName = e.UserName
                })
                .ToListAsync();
        }
    }
}
