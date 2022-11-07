using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Ticket;
using ProjectTracker.Core.ViewModels.Ticket.TicketComment;
using System.Security.Claims;

namespace ProjectTracker.Controllers
{
    public class TicketsController : BaseController
    {
        private readonly ITicketService ticketService;

        public TicketsController(ITicketService _ticketService)
        {
            ticketService = _ticketService;
        }

        public async Task<IActionResult> All()
        {
            var model = await ticketService.GetAll();

            return View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var model = await ticketService.GetTicketDetaisById(id);

            ViewBag.TicketId = model.Id;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await ticketService.GetById(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTicketViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await ticketService.EditTicket(model);

            return RedirectToAction(nameof(Details), new { id = model.Id });
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
