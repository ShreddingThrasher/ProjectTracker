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

        public async Task EditAsync(EditDepartmentViewModel model)
        {
            var department = await repo.All<Department>()
                .Where(d => d.IsActive && d.Id == model.Id)
                .FirstOrDefaultAsync();

            if(department == null)
            {
                throw new NullReferenceException();
            }

            department.Name = model.Name;
            department.LeadId = model.LeadId;

            await repo.SaveChangesAsync();
        }

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

        public async Task<Department> GetByIdAsync(Guid id)
            => await repo.All<Department>()
                .Where(d => d.IsActive)
                .FirstOrDefaultAsync(d => d.Id == id);

        public async Task<int> GetCountAsync()
            => await this.repo.AllReadonly<Department>().Where(d => d.IsActive).CountAsync();

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
    }
}
