using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Ticket
{
    public class AdminTicketViewModel
    {
        public Guid Id { get; set; }

        public string Submitter { get; set; }

        public string Title { get; set; }

        public string Project { get; set; }

        public string Department { get; set; }

        public string Priority { get; set; }

        public string Status { get; set; }

        public DateTime Date { get; set; }
    }
}
