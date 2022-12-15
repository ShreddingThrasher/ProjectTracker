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
    public class ProjectService : IProjectService
    {
        private readonly IRepository repo;

        public ProjectService(IRepository _repo)
        {
            repo = _repo;
        }


        /// <summary>
        /// Creates new Project
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        public async Task CreateAsync(CreateProjectViewModel model)
        {
            var department = await repo.All<Department>()
                .Where(d => d.IsActive)
                .FirstOrDefaultAsync(d => d.Id == model.DepartmentId);

            if(department == null)
            {
                throw new NullReferenceException("Department doesn't exist");
            }

            var project = new Project()
            {
                Name = model.Name,
                Description = model.Description,
                Department = department,
                IsActive = true
            };

            await repo.AddAsync(project);
            await repo.SaveChangesAsync();
        }


        /// <summary>
        /// Deletes Project
        /// </summary>
        /// <param name="id">ProjectId</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">Throws if Project doesn't exist.</exception>
        public async Task DeleteAsync(Guid id)
        {
            var project = await repo.All<Project>()
                .Where(p => p.IsActive && p.Id == id)
                .Include(p => p.AssignedEmployees)
                .Include(p => p.Tickets)
                .FirstOrDefaultAsync();

            if(project == null)
            {
                throw new NullReferenceException();
            }

            project.IsActive = false;

            foreach (var item in project.AssignedEmployees)
            {
                item.IsActive = false;
            }

            foreach (var item in project.Tickets)
            {
                item.IsActive = false;
            }

            await repo.SaveChangesAsync();
        }


        /// <summary>
        /// Edits Project
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>ProjectId</returns>
        /// <exception cref="NullReferenceException">Throws if Project doesn't exist.</exception>
        public async Task<Guid> EditProjectAsync(EditProjectViewModel model)
        {
            var project = await repo.All<Project>()
                .Where(p => p.IsActive && p.Id == model.Id)
                .FirstOrDefaultAsync();

            if(project == null)
            {
                throw new NullReferenceException();
            }

            project.Name = model.Name;
            project.Description = model.Description;

            await repo.SaveChangesAsync();
            return project.Id;
        }


        /// <summary>
        /// Gets all active Projects
        /// </summary>
        /// <returns>All active projects as ProjectViewModel</returns>
        public async Task<IEnumerable<ProjectViewModel>> GetAllProjectsAsync()
        {
            return await repo.AllReadonly<Project>()
                .Where(p => p.IsActive)
                .Select(p => new ProjectViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Department = p.Department.Name,
                    AssignedEmployeesCount = p.AssignedEmployees
                        .Where(ae => ae.IsActive)
                        .Count(),
                    TicketsCount = p.Tickets
                        .Where(t => t.IsActive)
                        .Count()
                })
                .ToListAsync();
        }


        /// <summary>
        /// Gets the Count of all active projects.
        /// </summary>
        /// <returns>Count as int</returns>
        public async Task<int> GetCountAsync()
            => await this.repo.AllReadonly<Project>().Where(p => p.IsActive).CountAsync();


        /// <summary>
        /// Gets details for a Project to be edited
        /// </summary>
        /// <param name="id">ProjectId</param>
        /// <returns>Project details</returns>
        /// <exception cref="NullReferenceException">Throws if the Project doesn't exist.</exception>
        public async Task<EditProjectViewModel> GetEditDetailsAsync(Guid id)
        {
            var project = await repo.AllReadonly<Project>()
                .Where(p => p.Id == id)
                .Select(p => new EditProjectViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                })
                .FirstOrDefaultAsync();

            if(project == null)
            {
                throw new NullReferenceException();
            }

            return project;
        }


        /// <summary>
        /// Gets Id and Name for all active Projects
        /// </summary>
        /// <returns>All projects Id and Name</returns>
        public async Task<IEnumerable<ProjectIdNameViewModel>> GetIdsAndNamesAsync()
        {
            return await repo.AllReadonly<Project>()
                .Where(p => p.IsActive)
                .Select(p => new ProjectIdNameViewModel()
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToListAsync();
        }


        /// <summary>
        /// Gets all innactive Projects
        /// </summary>
        /// <returns>All innactive projects as ProjectViewModel</returns>
        public async Task<IEnumerable<ProjectViewModel>> GetInactiveProjectsAsync()
        {
            return await repo.AllReadonly<Project>()
                .Where(p => !p.IsActive)
                .Select(p => new ProjectViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Department = p.Department.Name,
                    AssignedEmployeesCount = p.AssignedEmployees.Count(),
                    TicketsCount = p.Tickets.Count()
                })
                .ToListAsync();
        }


        /// <summary>
        /// Gets details for an active Project
        /// </summary>
        /// <param name="id">ProjectId</param>
        /// <returns>Project details</returns>
        /// <exception cref="NullReferenceException">Throws if the project doesn't exist</exception>
        public async Task<ProjectDetailsViewModel> GetProjectDetailsByIdAsync(Guid id)
        {
            var project = await repo.AllReadonly<Project>()
                .Where(p => p.Id == id && p.IsActive)
                .Include(p => p.Department)
                .Include(p => p.AssignedEmployees)
                .ThenInclude(ae => ae.Employee)
                .Include(p => p.Tickets)
                .ThenInclude(t => t.Submitter)
                .FirstOrDefaultAsync();

            if(project == null)
            {
                throw new NullReferenceException();
            }

            return new ProjectDetailsViewModel()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Department = new DepartmentIdNameViewModel()
                {
                    Id = project.Department.Id,
                    Name = project.Department.Name
                },
                Employees = project.AssignedEmployees
                    .Where(ae => ae.IsActive)
                    .Select(ae => new EmployeeViewModel()
                    {
                        Id = ae.Employee.Id,
                        FullName = ae.Employee.FirstName + " " + ae.Employee.LastName,
                        Email = ae.Employee.Email,
                    }).ToList(),
                Tickets = project.Tickets
                    .Where(t => t.IsActive)
                    .Select(t => new TicketViewModel()
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Submitter = new EmployeeIdNameViewModel()
                        {
                            Id = t.Submitter.Id,
                            UserName = t.Submitter.UserName
                        },
                        Priority = t.Priority,
                        Status = t.Status,
                        Date = t.CreatedOn
                    }).ToList()
            };
        }


        /// <summary>
        /// Get all Projects that the user is assigned to.
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns>Collection of ProjectViewModel</returns>
        public async Task<IEnumerable<ProjectViewModel>> UserProjectsAsync(string userId)
        {
            var model = await repo.AllReadonly<EmployeeProject>()
                .Where(ep => ep.IsActive && ep.EmployeeId == userId)
                .Include(ep => ep.Project)
                .ThenInclude(p => p.Department)
                .Select(ep => ep.Project)
                .Select(p => new ProjectViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Department = p.Department.Name,
                    AssignedEmployeesCount = p.AssignedEmployees.Count(),
                    TicketsCount = p.Tickets.Count()
                })
                .ToListAsync();

            return model;
        }
    }
}
