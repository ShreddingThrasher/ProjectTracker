using ProjectTracker.Core.ViewModels.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Contracts
{
    public interface IEmployeeService
    {
        Task<int> GetCountAsync();

        Task<IEnumerable<EmployeeViewModel>> GetAllAsync();

        Task<IEnumerable<EmployeeViewModel>> GetActiveAsync();

        Task<IEnumerable<EmployeeViewModel>> GetUnassignedAsync();

        Task<IEnumerable<string>> GetUserNamesAsync();

        Task<IEnumerable<EmployeeIdNameViewModel>> GetAllIdAndNameAsync();

        Task<EmployeeDetailsViewModel> GetEmployeeDetailsAsync(string id);

        Task RemoveById(string employeeId);
    }
}
