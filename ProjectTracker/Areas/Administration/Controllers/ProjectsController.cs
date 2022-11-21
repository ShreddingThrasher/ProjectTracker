using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Project;
using ProjectTracker.Core.ViewModels.Ticket;
using ProjectTracker.Infrastructure.Data.Entities.Enums;
using System.Security.Claims;

namespace ProjectTracker.Areas.Administration.Controllers
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

            return Redirect("/Projects/All");
        }

    }
}
