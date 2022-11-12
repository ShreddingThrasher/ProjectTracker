using ProjectTracker.Core.ViewModels.Employee;
using ProjectTracker.Infrastructure.DataConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Department
{
    public class CreateDepartmentViewModel
    {
        public CreateDepartmentViewModel()
        {
            Employees = new List<EmployeeIdNameViewModel>();
        }

        [Required]
        [StringLength(DepartmentConstants.NameMaxLength,
            MinimumLength = DepartmentConstants.NameMinLength)]
        public string Name { get; set; }

        [Required]
        public string LeadId { get; set; }

        public IEnumerable<EmployeeIdNameViewModel> Employees { get; set; }
    }
}
