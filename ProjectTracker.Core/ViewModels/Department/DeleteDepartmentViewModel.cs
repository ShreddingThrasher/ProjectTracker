using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Department
{
    public class DeleteDepartmentViewModel
    {
        public DeleteDepartmentViewModel()
        {
            Departments = new List<DepartmentIdNameViewModel>();
        }

        public Guid Id { get; set; }

        public IEnumerable<DepartmentIdNameViewModel> Departments { get; set; }
    }
}
