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
        Task<int> GetCount();

        Task<IEnumerable<EmployeeViewModel>> GetAll();

        Task<IEnumerable<string>> GetUserNamesAsync();

        Task<IEnumerable<EmployeeIdNameViewModel>> GetAllIdAndNameAsync();
    }
}
