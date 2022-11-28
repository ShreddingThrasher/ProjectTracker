using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Project;
using ProjectTracker.Core.ViewModels.Ticket;
using ProjectTracker.Infrastructure.Data.Entities.Enums;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectTracker.Areas.Administration.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IProjectService projectService;
        private readonly IDepartmentService departmentService;
        private readonly ITicketService ticketService;

        public ProjectController(
            IProjectService _projectService,
            IDepartmentService _departmentService,
            ITicketService _ticketService)
        {
            projectService = _projectService;
            departmentService = _departmentService;
            ticketService = _ticketService;
        }

        public async Task<IActionResult> Active()
        {
            var model = await projectService.GetAllProjectsAsync();

            return View(model);
        }

        public async Task<IActionResult> Past()
        {
            var model = await projectService.GetInactiveProjectsAsync();

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

            await projectService.CreateAsync(model);

            return Redirect("/Project/All");
        }

        [HttpGet]
        public async Task<IActionResult> Change(bool? success)
        {
            var model = new ChangeProjectViewModel()
            {
                Projects = await projectService.GetIdsAndNamesAsync()
            };

            ViewBag.Success = success;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var model = await projectService.GetEditDetailsAsync(id);

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Change), new { success = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var id = await projectService.EditProjectAsync(model);

                return Redirect($"/Project/Details/{id}");
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Change), new { success = false});
            }
        }

        [HttpGet]
        public async Task<IActionResult> Remove()
        {
            var model = new DeleteProjectViewModel()
            {
                Projects = await projectService.GetIdsAndNamesAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(DeleteProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");

                model.Projects = await projectService.GetIdsAndNamesAsync();


                return View(model);
            }

            try
            {
                await projectService.DeleteAsync(model.Id);

                return Redirect("/Project/All");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");

                model.Projects = await projectService.GetIdsAndNamesAsync();

                return View(model);
            }
        }
    }
}
