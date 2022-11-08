using ProjectTracker.Core.ViewModels.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Ticket
{
    public class AssignTicketViewModel
    {
        public AssignTicketViewModel()
        {
            Employees = new List<EmployeeIdNameViewModel>();
        }

        [Required]
        public Guid TicketId { get; set; }

        [Required]
        public string EmployeeId { get; set; }

        public IEnumerable<EmployeeIdNameViewModel> Employees { get; set; }
    }
}
