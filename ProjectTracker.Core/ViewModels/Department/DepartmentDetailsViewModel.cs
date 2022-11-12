using ProjectTracker.Core.ViewModels.Employee;
using ProjectTracker.Core.ViewModels.Project;
using ProjectTracker.Core.ViewModels.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Department
{
    public class DepartmentDetailsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public EmployeeViewModel Leader { get; set; }

        public IEnumerable<EmployeeViewModel> Employees { get; set; }

        public IEnumerable<ProjectViewModel> Projects { get; set; }

        public IEnumerable<TicketViewModel> Tickets { get; set; }
    }
}
