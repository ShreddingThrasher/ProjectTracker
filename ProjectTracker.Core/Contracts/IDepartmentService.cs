using ProjectTracker.Core.ViewModels.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Contracts
{
    public interface IDepartmentService
    {
        Task<int> GetCount();

        Task<IEnumerable<DepartmentViewModel>> GetAll();

        Task<IEnumerable<CreateProjectDepartmentModel>> GetAllIdAndName();
    }
}
