using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Ticket;
using ProjectTracker.Core.ViewModels.Ticket.TicketComment;
using System.Security.Claims;

namespace ProjectTracker.Controllers
{
    public class TicketController : BaseController
    {
        private readonly ITicketService ticketService;
        private readonly IEmployeeService employeeService;

        public TicketController(
            ITicketService _ticketService,
            IEmployeeService _employeeService)
        {
            ticketService = _ticketService;
            employeeService = _employeeService;
        }

        public async Task<IActionResult> All()
        {
            var model = await ticketService.GetAllAsync();

            return View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var model = await ticketService.GetTicketDetailsByIdAsync(id);

            if (model == null)
            {
                return RedirectToAction(nameof(HomeController.NotFound), "Home");
            }

            ViewBag.TicketId = model.Id;

            return View(model);
        }

        public async Task<IActionResult> Assigned()
        {
            var model = await ticketService.UserAssignedTicketsAsync(User.Id());

            return View(model);
        }

        public async Task<IActionResult> My()
        {
            var model = await ticketService.UserSubmittedAsync(User.Id());

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CreateTicketCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Details), new { id = model.TicketId});
            }

            try
            {
                await ticketService.CreateCommentAsync(User.Id(), model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(All));
            }

            return RedirectToAction(nameof(Details), new { id = model.TicketId });
        }
    }
}
