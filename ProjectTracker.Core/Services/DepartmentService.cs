using Microsoft.EntityFrameworkCore;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Department;
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

        public async Task<IEnumerable<DepartmentViewModel>> GetAll()
        {
            return await repo.AllReadonly<Department>()
                .Where(d => d.IsActive)
                .Select(d => new DepartmentViewModel()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Employees = d.Employees.Count,
                    Projects = d.Projects.Count,
                    Tickets = d.Tickets.Count,
                    LeaderId = d.LeadId
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CreateProjectDepartmentModel>> GetAllIdAndName()
        {
            return await repo.AllReadonly<Department>()
                .Where(d => d.IsActive)
                .Select(d => new CreateProjectDepartmentModel()
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToListAsync();
        }

        public async Task<int> GetCount()
            => this.repo.AllReadonly<Department>().Where(d => d.IsActive).Count();
    }
}
