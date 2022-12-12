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
        /// <summary>
        /// Gets the count of all current Employees
        /// </summary>
        /// <returns>Count as int</returns>
        Task<int> GetCountAsync();


        /// <summary>
        /// Gets all current Employees
        /// </summary>
        /// <returns>Collection of EmployeeViewModel</returns>
        Task<IEnumerable<EmployeeViewModel>> GetAllAsync();


        /// <summary>
        /// Gets all Employees that are assigned to a Department
        /// </summary>
        /// <returns>Collection of EmployeeViewModel</returns>
        Task<IEnumerable<EmployeeViewModel>> GetActiveAsync();


        /// <summary>
        /// Gets all Employees that are not assigned to a Department
        /// </summary>
        /// <returns>Collection of EmployeeViewModel</returns>
        Task<IEnumerable<EmployeeViewModel>> GetUnassignedAsync();


        /// <summary>
        /// Gets the UserNames for all currentEmployees
        /// </summary>
        /// <returns>Collection of string</returns>
        Task<IEnumerable<string>> GetUserNamesAsync();


        /// <summary>
        /// Gets Id and UserName for all current Employees
        /// </summary>
        /// <returns>Collection of EmployeeIdNameViewModel</returns>
        Task<IEnumerable<EmployeeIdNameViewModel>> GetAllIdAndNameAsync();


        /// <summary>
        /// Gets details for a given Employee
        /// </summary>
        /// <param name="id">EmployeeId</param>
        /// <returns>Model holding detailed data for the Employee</returns>
        Task<EmployeeDetailsViewModel> GetEmployeeDetailsAsync(string id);


        /// <summary>
        /// Sets an Employee to innactive
        /// </summary>
        /// <param name="employeeId">EmployeeId</param>
        /// <returns></returns>
        Task RemoveById(string employeeId);
    }
}
