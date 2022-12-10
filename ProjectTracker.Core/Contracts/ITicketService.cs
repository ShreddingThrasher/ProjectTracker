using ProjectTracker.Core.ViewModels.Ticket;
using ProjectTracker.Core.ViewModels.Ticket.TicketComment;
using ProjectTracker.Infrastructure.Data.Entities.Enums;
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

        Task<IEnumerable<TicketViewModel>> GetAllAsync();

        Task<IEnumerable<AdminTicketViewModel>> GetInProgressAsync();

        Task<IEnumerable<AdminTicketViewModel>> GetDoneAsync();

        Task<IEnumerable<AdminTicketViewModel>> GetUnassignedAsync();

        Task<IEnumerable<AdminTicketViewModel>> GetPastAsync();

        Task AssignTicketAsync(AssignTicketViewModel model);

        Task EditTicketAsync(EditTicketViewModel model);

        Task<EditTicketViewModel> GetByIdAsync(Guid id);

        Task CreateTicketAsync(SubmitTicketViewModel model, string submitterId, Guid departmentId);

        Task<TicketDetailsViewModel> GetTicketDetaisByIdAsync(Guid id);

        Task CreateCommentAsync(string userId, CreateTicketCommentViewModel model);

        Task<IEnumerable<Status>> GetAllStatusesAsync();
    }
}
