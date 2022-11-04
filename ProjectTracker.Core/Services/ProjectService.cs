using Microsoft.EntityFrameworkCore;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Project;
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
            => this.repo.AllReadonly<Project>().Where(p => p.IsActive).Count();
    }
}
