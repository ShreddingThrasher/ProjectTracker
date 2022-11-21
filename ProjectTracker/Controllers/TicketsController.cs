using HouseRentingSystem.Extensions;
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
    public class TicketsController : BaseController
    {
        private readonly ITicketService ticketService;
        private readonly IEmployeeService employeeService;

        public TicketsController(
            ITicketService _ticketService,
            IEmployeeService _employeeService)
        {
            ticketService = _ticketService;
            employeeService = _employeeService;
        }

        public async Task<IActionResult> All()
        {
            var model = await ticketService.GetAll();

            return View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var model = await ticketService.GetTicketDetaisById(id);

            if (model == null)
            {
                return RedirectToAction(nameof(HomeController.NotFound), "Home");
            }

            ViewBag.TicketId = model.Id;

            return View(model);
        }

        [HttpPost]
        [Route("/Tickets/Details/{Id}/Comment")]
        public async Task<IActionResult> Comment(Guid Id, CreateTicketCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Details), new { id = Id});
            }

            await ticketService.CreateComment(User.Id(), Id, model);

            return RedirectToAction(nameof(Details), new { id = Id });
        }
    }
}
