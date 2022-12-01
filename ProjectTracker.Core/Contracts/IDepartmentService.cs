using ProjectTracker.Core.ViewModels.Department;
using ProjectTracker.Core.ViewModels.Employee;
using ProjectTracker.Core.ViewModels.Project;
using ProjectTracker.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Contracts
{
    public interface IDepartmentService
    {
        Task<int> GetCountAsync();

        Task<IEnumerable<DepartmentViewModel>> GetAllAsync();

        Task<IEnumerable<DepartmentViewModel>> GetInactiveDepartmentsAsync();

        Task<IEnumerable<DepartmentIdNameViewModel>> GetAllIdAndNameAsync();

        Task<Department> GetByIdAsync(Guid id);

        Task CreateAsync(CreateDepartmentViewModel model);

        Task<DepartmentDetailsViewModel> GetDepartmentDetailsAsync(Guid departmentId);

        Task<EditDepartmentViewModel> GetEditDetailsAsync(Guid id);

        Task EditAsync(EditDepartmentViewModel model);

        Task DeleteAsync(Guid id);

        Task<IEnumerable<EmployeeIdNameViewModel>> GetPosibleLeadersAsync();
    }
}
