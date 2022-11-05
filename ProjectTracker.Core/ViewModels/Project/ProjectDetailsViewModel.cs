using ProjectTracker.Core.ViewModels.Department;
using ProjectTracker.Core.ViewModels.Employee;
using ProjectTracker.Core.ViewModels.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Project
{
    public class ProjectDetailsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DepartmentIdNameViewModel Department { get; set; }

        public IEnumerable<EmployeeViewModel> Employees { get; set; }

        public IEnumerable<TicketViewModel> Tickets { get; set; }
    }
}
