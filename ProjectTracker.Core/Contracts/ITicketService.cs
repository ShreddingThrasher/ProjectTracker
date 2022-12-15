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
        /// <summary>
        /// Gets the Count of all active Tickets
        /// </summary>
        /// <returns>Count as int</returns>
        Task<int> GetCountAsync();


        /// <summary>
        /// Gets all active Tickets
        /// </summary>
        /// <returns>Collection of TicketViewModel</returns>
        Task<IEnumerable<TicketViewModel>> GetAllAsync();


        /// <summary>
        /// Gets all Tickets with Status - InProgress
        /// </summary>
        /// <returns>Tickets</returns>
        Task<IEnumerable<AdminTicketViewModel>> GetInProgressAsync();


        /// <summary>
        /// Gets all Tickets with Status - Done
        /// </summary>
        /// <returns>Tickets</returns>
        Task<IEnumerable<AdminTicketViewModel>> GetDoneAsync();


        /// <summary>
        /// Gets all Tickets with Status - Open
        /// </summary>
        /// <returns>Tickets</returns>
        Task<IEnumerable<AdminTicketViewModel>> GetUnassignedAsync();


        /// <summary>
        /// Gets all innactive Tickets
        /// </summary>
        /// <returns>Tickets</returns>
        Task<IEnumerable<AdminTicketViewModel>> GetPastAsync();


        /// <summary>
        /// Assigns Ticket to Employee
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        Task AssignTicketAsync(AssignTicketViewModel model);


        /// <summary>
        /// Edits Ticket
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        Task EditTicketAsync(EditTicketViewModel model);


        /// <summary>
        /// Gets details for the Ticket to be edited.
        /// </summary>
        /// <param name="id">TicketId</param>
        /// <returns>Ticket details</returns>
        Task<EditTicketViewModel> GetEditDetailsById(Guid id);

        Task CreateTicketAsync(SubmitTicketViewModel model, string submitterId, Guid departmentId);

        Task<TicketDetailsViewModel> GetTicketDetailsByIdAsync(Guid id);

        Task CreateCommentAsync(string userId, CreateTicketCommentViewModel model);

        Task<IEnumerable<Status>> GetAllStatusesAsync();

        Task<IEnumerable<TicketViewModel>> UserAssignedTicketsAsync(string userId);

        Task<IEnumerable<TicketViewModel>> UserSubmittedAsync(string userId);
    }
}
