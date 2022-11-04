using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Project;

namespace ProjectTracker.Controllers
{
    public class ProjectsController : BaseController
    {
        private readonly IProjectService projectService;
        private readonly IDepartmentService departmentService;

        public ProjectsController(
            IProjectService _projectService,
            IDepartmentService _departmentService)
        {
            projectService = _projectService;
            departmentService = _departmentService;
        }

        public async Task<IActionResult> All()
        {
            var model = await projectService.GetAllProjects();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateProjectViewModel()
            {
                Departments = await departmentService.GetAllIdAndName()
            };

            return View(model);
        }
    }
}
