using ProjectTracker.Core.ViewModels.Ticket;
using ProjectTracker.Core.ViewModels.Ticket.TicketComment;
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

        Task EditTicket(EditTicketViewModel model);

        Task<EditTicketViewModel> GetById(Guid id);

        Task CreateTicketAsync(SubmitTicketViewModel model, string submitterId, Guid departmentId);

        Task<TicketDetailsViewModel> GetTicketDetaisById(Guid id);

        Task CreateComment(string userId, Guid ticketId, CreateTicketCommentViewModel model);
    }
}
