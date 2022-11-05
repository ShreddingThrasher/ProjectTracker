using ProjectTracker.Core.ViewModels.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ProjectTracker.Core.Contracts
{
    public interface ITicketService
    {
        Task<int> GetCount();

        Task<IEnumerable<TicketViewModel>> GetAll();

        Task CreateTicketAsync(SubmitTicketViewModel model, string submitterId, Guid departmentId);

        Task<TicketDetailsViewModel> GetTicketDetaisById(Guid id);
    }
}
