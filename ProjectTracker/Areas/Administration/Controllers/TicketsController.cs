using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Ticket;
using ProjectTracker.Core.ViewModels.Ticket.TicketComment;
using System.Security.Claims;

namespace ProjectTracker.Areas.Administration.Controllers
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

            return Redirect($"/Tickets/Details/{model.Id}");
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
        [Route("/Administration/Tickets/Details/{Id}/Assign")]
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

                return Redirect($"/Tickets/Details/{model.TicketId}");
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
