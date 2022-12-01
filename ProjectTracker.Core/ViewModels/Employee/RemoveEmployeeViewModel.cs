using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Employee
{
    public class RemoveEmployeeViewModel
    {
        public RemoveEmployeeViewModel()
        {
            this.Employees = new List<EmployeeIdNameViewModel>();
        }

        public string Id { get; set; }

        public IEnumerable<EmployeeIdNameViewModel> Employees { get; set; }
    }
}
