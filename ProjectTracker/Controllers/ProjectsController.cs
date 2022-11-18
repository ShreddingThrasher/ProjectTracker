using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Project;
using ProjectTracker.Core.ViewModels.Ticket;
using ProjectTracker.Infrastructure.Data.Entities.Enums;
using System.Security.Claims;

namespace ProjectTracker.Controllers
{
    public class ProjectsController : BaseController
    {
        private readonly IProjectService projectService;
        private readonly IDepartmentService departmentService;
        private readonly ITicketService ticketService;

        public ProjectsController(
            IProjectService _projectService,
            IDepartmentService _departmentService,
            ITicketService _ticketService)
        {
            projectService = _projectService;
            departmentService = _departmentService;
            ticketService = _ticketService;
        }

        public async Task<IActionResult> All()
        {
            var model = await projectService.GetAllProjects();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(bool? success)
        {
            var model = new CreateProjectViewModel()
            {
                Departments = await departmentService.GetAllIdAndNameAsync()
            };

            ViewBag.Success = success;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await projectService.Create(model);

            //TODO: Redirect to details
            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var model = await projectService.GetProjectDetailsById(id);

            if (model == null)
            {
                return RedirectToAction(nameof(HomeController.NotFound), "Home");
            }

            ViewBag.Open = model.Tickets.Where(t => t.Status == Status.Open).Count();
            ViewBag.InProgress = model.Tickets.Where(t => t.Status == Status.InProgress).Count();
            ViewBag.Done = model.Tickets.Where(t => t.Status == Status.Done).Count();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SubmitTicket(Guid id, bool? success)
        {
            var model = new SubmitTicketViewModel();
            model.ProjectId = id;

            ViewBag.Success = success;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitTicket(SubmitTicketViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var submitterId = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var project = await projectService.GetProjectDetailsById(model.ProjectId);

            try
            {
                await ticketService.CreateTicketAsync(model, submitterId, project.Department.Id);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(SubmitTicket), new { Success = false });
            }

            return RedirectToAction(nameof(Details), new { Id = model.ProjectId });
        }
    }
}
