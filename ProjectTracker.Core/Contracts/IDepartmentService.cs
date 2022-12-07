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
        /// <summary>
        /// Gets the count of all active departments in the database
        /// </summary>
        /// <returns>Count as int32</returns>
        Task<int> GetCountAsync();

        /// <summary>
        /// Gets all active departments in the database
        /// </summary>
        /// <returns>IEnumerable<DepartmentViewModel></returns>
        Task<IEnumerable<DepartmentViewModel>> GetAllAsync();

        /// <summary>
        /// Gets all inactive departments in the database
        /// </summary>
        /// <returns>IEnumerable<DepartmentViewModel></returns>
        Task<IEnumerable<DepartmentViewModel>> GetInactiveDepartmentsAsync();

        /// <summary>
        /// Gets Id and Name for all active departments in the database
        /// </summary>
        /// <returns>IEnumerable<DepartmentIdNameViewModel></returns>
        Task<IEnumerable<DepartmentIdNameViewModel>> GetAllIdAndNameAsync();

        /// <summary>
        /// Creates a new Department async
        /// </summary>
        /// <param name="model">View model containing the new Department data</param>
        /// <returns></returns>
        Task CreateAsync(CreateDepartmentViewModel model);

        /// <summary>
        /// Gets details for an Active Department by Id async
        /// </summary>
        /// <param name="departmentId">Department Id</param>
        /// <returns>DepartmentDetailsViewModel or null</returns>
        Task<DepartmentDetailsViewModel> GetDepartmentDetailsAsync(Guid departmentId);

        /// <summary>
        /// Gets details for a Department to be edited.
        /// </summary>
        /// <param name="id">Department Id</param>
        /// <returns>EditDepartmentViewModel</returns>
        Task<EditDepartmentViewModel> GetEditDetailsAsync(Guid id);

        /// <summary>
        /// Edits existing Department.
        /// </summary>
        /// <param name="model">Model with the Edit data</param>
        /// <returns></returns>
        Task EditAsync(EditDepartmentViewModel model);

        /// <summary>
        /// Deletes Department
        /// </summary>
        /// <param name="id">Department Id</param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Gets all Employees that can be set as Department Leader
        /// </summary>
        /// <returns>Id and Name for all Employees that are currently not a leader of another Department</returns>
        Task<IEnumerable<EmployeeIdNameViewModel>> GetPossibleLeadersAsync();
    }
}
