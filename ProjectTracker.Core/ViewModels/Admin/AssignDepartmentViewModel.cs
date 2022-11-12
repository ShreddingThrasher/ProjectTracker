using ProjectTracker.Core.ViewModels.Department;
using ProjectTracker.Core.ViewModels.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Admin
{
    public class AssignDepartmentViewModel
    {
        public AssignDepartmentViewModel()
        {
            Employees = new List<EmployeeIdNameViewModel>();
            Departments = new List<DepartmentIdNameViewModel>();
        }

        [Required]
        [Display(Name = "Employee")]
        public string EmployeeId { get; set; }

        public IEnumerable<EmployeeIdNameViewModel> Employees { get; set; }

        [Required]
        [Display(Name = "Department")]
        public Guid DepartmentId { get; set; }

        public IEnumerable<DepartmentIdNameViewModel> Departments { get; set; }
    }
}
