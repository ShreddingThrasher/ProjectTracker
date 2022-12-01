using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Department
{
    public class ChangeDepartmentViewModel
    {
        [Required]
        [Display(Name = "Department")]
        public Guid DepartmentId { get; set; }

        public IEnumerable<DepartmentIdNameViewModel> Departments { get; set; }
    }
}
