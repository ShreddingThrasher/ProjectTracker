using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Constants;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Admin;

namespace ProjectTracker.Controllers
{
    [Authorize(Roles = RoleConstants.Admin)]
    public class AdminController : BaseController
    {
        private readonly IAdminService adminService;
        private readonly IEmployeeService employeeService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IProjectService projectService;

        public AdminController(
            IAdminService _adminService,
            IEmployeeService _employeeService,
            RoleManager<IdentityRole> _roleManager,
            IProjectService _projectService)
        {
            adminService = _adminService;
            employeeService = _employeeService;
            roleManager = _roleManager;
            projectService = _projectService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Test()
        {
            var role = await roleManager.FindByNameAsync(RoleConstants.Admin);

            string name = role.Name;

            var x = User.IsInRole(RoleConstants.Admin);
            var y = User.IsInRole("nonexistent");


            Console.WriteLine();

            return View();
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await adminService.CreateRoleAsync(model);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AssignRoles(bool? success)
        {
            var model = new AssignRolesViewModel()
            {
                Roles = await adminService.GetAllRolesAsync(),
                Employees = await employeeService.GetUserNamesAsync()
            };

            ViewBag.Success = success;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRoles(AssignRolesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await adminService.AddToRoleAsync(model.Employee, model.Role);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(AssignRoles), new { Success = true });
                }

            }
            catch (Exception)
            {
                return RedirectToAction(nameof(AssignRoles), new { Success = false });
            }

            return RedirectToAction(nameof(AssignRoles), new { Success = false });
        }

        [HttpGet]
        public async Task<IActionResult> AssignProjects()
        {
            var model = new AssignProjectsViewModel()
            {
                Employees = await employeeService.GetIdsAndNamesAsync(),
                Projects = await projectService.GetIdsAndNamesAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignProjects(AssignProjectsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Employees = await employeeService.GetIdsAndNamesAsync();
                model.Projects = await projectService.GetIdsAndNamesAsync();

                return View(model);
            }

            try
            {
                await adminService.AssignToProjectAsync(model.EmployeeId, model.ProjectId);

                RedirectToAction("Detauks", "Projects", new { id = model.ProjectId });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, "The Employee is already assigned to this project.");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong :(");
            }

            model.Employees = await employeeService.GetIdsAndNamesAsync();
            model.Projects = await projectService.GetIdsAndNamesAsync();

            return View(model);
        }
    }
}
