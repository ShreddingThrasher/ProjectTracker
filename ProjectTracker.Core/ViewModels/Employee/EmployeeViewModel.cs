using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Employee
{
    public class EmployeeViewModel
    {
        public string FullName { get; set; }
        public string Department { get; set; }
        public int AssignedProjects { get; set; }
        public int AssignedTickets { get; set; }
        public string Email { get; set; }
    }
}
