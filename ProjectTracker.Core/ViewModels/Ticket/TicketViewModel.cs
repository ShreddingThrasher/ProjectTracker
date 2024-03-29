﻿using ProjectTracker.Infrastructure.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTracker.Core.ViewModels.Project;
using ProjectTracker.Core.ViewModels.Department;
using ProjectTracker.Core.ViewModels.Employee;

namespace ProjectTracker.Core.ViewModels.Ticket
{
    public class TicketViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public ProjectIdNameViewModel Project { get; set; }

        public DepartmentIdNameViewModel Department { get; set; }

        public EmployeeIdNameViewModel Submitter { get; set; }

        public Priority Priority { get; set; }

        public Status Status { get; set; }

        public DateTime Date { get; set; }
    }
}
