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

            ViewBag.TicketId = model.Id;

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "CanAssignAndEditTicket")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await ticketService.GetById(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "CanAssignAndEditTicket")]
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

        [HttpGet]
        [Authorize(Policy = "CanAssignAndEditTicket")]
        [Route("/Tickets/Details/{Id}/Assign")]
        public async Task<IActionResult> Assign(Guid id)
        {
            var model = new AssignTicketViewModel()
            {
                TicketId = id,
                Employees = await employeeService.GetAllIdAndNameAsync()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "CanAssignAndEditTicket")]
        [Route("/Tickets/Details/{Id}/Assign")]
        public async Task<IActionResult> Assign(AssignTicketViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Employees = await employeeService.GetAllIdAndNameAsync();

                return View(model);
            }

            try
            {
                await ticketService.AssignTicket(model);

                return RedirectToAction(nameof(Details), new { id = model.TicketId });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (NullReferenceException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            model.Employees = await employeeService.GetAllIdAndNameAsync();

            return View(model);
        }
    }
}
