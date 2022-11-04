using ProjectTracker.Core.ViewModels.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Project
{
    public class ProjectViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Department { get; set; }

        public int AssignedEmployeesCount { get; set; }

        public int TicketsCount { get; set; }
    }
}
