﻿using Microsoft.EntityFrameworkCore;
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

        public async Task Create(CreateProjectViewModel model)
        {
            var department = await repo.All<Department>()
                .Where(d => d.IsActive)
                .FirstOrDefaultAsync(d => d.Id == model.DepartmentId);

            var project = new Project()
            {
                Name = model.Name,
                Description = model.Description,
                Department = department
            };

            await repo.AddAsync(project);
            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProjectViewModel>> GetAllProjects()
        {
            return await repo.AllReadonly<Project>()
                .Where(p => p.IsActive)
                .Select(p => new ProjectViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Department = p.Department.Name,
                    AssignedEmployeesCount = p.AssignedEmployees.Count,
                    TicketsCount = p.Tickets.Count
                })
                .ToListAsync();
        }

        public async Task<int> GetCount()
            => await this.repo.AllReadonly<Project>().Where(p => p.IsActive).CountAsync();

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

        public async Task<ProjectDetailsViewModel> GetProjectDetailsById(Guid id)
        {
            var project = await repo.AllReadonly<Project>()
                .Where(p => p.Id == id && p.IsActive)
                .Include(p => p.Department)
                .Include(p => p.AssignedEmployees)
                .ThenInclude(ae => ae.Employee)
                .Include(p => p.Tickets)
                .ThenInclude(t => t.Submitter)
                .FirstOrDefaultAsync();

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
                        Subbmitter = t.Submitter.UserName,
                        Priority = t.Priority,
                        Status = t.Status,
                        Date = t.CreatedOn
                    }).ToList()
            };
        }
    }
}
