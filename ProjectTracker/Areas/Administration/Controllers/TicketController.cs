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

        public async Task<IActionResult> InProgress()
        {
            var model = await ticketService.GetInProgressAsync();

            return View(model);
        }

        public async Task<IActionResult> Done()
        {
            var model = await ticketService.GetDoneAsync();

            return View(model);
        }

        public async Task<IActionResult> Unassigned()
        {
            var model = await ticketService.GetUnassignedAsync();

            return View(model);
        }

        public async Task<IActionResult> Past()
        {
            var model = await ticketService.GetPastAsync();

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "CanAssignAndEditTicket")]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var model = await ticketService.GetEditDetailsById(id);

                return View(model);
            }
            catch (NullReferenceException)
            {
                return RedirectToAction(nameof(AdminController.Index), "Admin");
            }
        }

        [HttpPost]
        [Authorize(Policy = "CanAssignAndEditTicket")]
        public async Task<IActionResult> Edit(EditTicketViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await ticketService.EditTicketAsync(model);
            }
            catch (NullReferenceException)
            {
                return RedirectToAction(nameof(AdminController.Index), "Admin");
            }

            return Redirect($"/Ticket/Details/{model.Id}");
        }

        [HttpGet]
        [Authorize(Policy = "CanAssignAndEditTicket")]
        [Route("/Administration/Ticket/Details/{Id}/Assign")]
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
        [Route("/Administration/Ticket/Details/{Id}/Assign")]
        public async Task<IActionResult> Assign(AssignTicketViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Employees = await employeeService.GetAllIdAndNameAsync();

                return View(model);
            }

            try
            {
                await ticketService.AssignTicketAsync(model);

                return Redirect($"/Ticket/Details/{model.TicketId}");
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
