using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Contracts;

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

            return View(model);
        }
    }
}
