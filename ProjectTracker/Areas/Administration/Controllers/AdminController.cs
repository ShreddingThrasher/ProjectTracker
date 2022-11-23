using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Controllers;
using ProjectTracker.Core.Constants;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Admin;
using System.Data;

namespace ProjectTracker.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = RoleConstants.Admin)]
    public class AdminController : BaseController
    {
        private readonly IAdminService adminService;
        private readonly IEmployeeService employeeService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IProjectService projectService;
        private readonly IDepartmentService departmentService;

        public AdminController(
            IAdminService _adminService,
            IEmployeeService _employeeService,
            RoleManager<IdentityRole> _roleManager,
            IProjectService _projectService,
            IDepartmentService _departmentService)
        {
            adminService = _adminService;
            employeeService = _employeeService;
            roleManager = _roleManager;
            projectService = _projectService;
            departmentService = _departmentService;
        }

        public IActionResult Index()
        {
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
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                model.Employees = await employeeService.GetUserNamesAsync();
                model.Roles = await adminService.GetAllRolesAsync();

                return View(model);
            }

            ModelState.AddModelError(string.Empty, "The employee is already in that Role.");

            model.Employees = await employeeService.GetUserNamesAsync();
            model.Roles = await adminService.GetAllRolesAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AssignDepartments()
        {
            var model = new AssignDepartmentViewModel()
            {
                Employees = await employeeService.GetAllIdAndNameAsync(),
                Departments = await departmentService.GetAllIdAndNameAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignDepartments(AssignDepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Employees = await employeeService.GetAllIdAndNameAsync();
                model.Departments = await departmentService.GetAllIdAndNameAsync();

                return View(model);
            }

            try
            {
                await adminService.AssignToDepartmentAsync(model.EmployeeId, model.DepartmentId);

                return Redirect($"/Department/Details/{model.DepartmentId}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            model.Employees = await employeeService.GetAllIdAndNameAsync();
            model.Departments = await departmentService.GetAllIdAndNameAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AssignProjects()
        {
            var model = new AssignProjectsViewModel()
            {
                Employees = await employeeService.GetAllIdAndNameAsync(),
                Projects = await projectService.GetIdsAndNamesAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignProjects(AssignProjectsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Employees = await employeeService.GetAllIdAndNameAsync();
                model.Projects = await projectService.GetIdsAndNamesAsync();

                return View(model);
            }

            try
            {
                await adminService.AssignToProjectAsync(model.EmployeeId, model.ProjectId);

                return Redirect($"/Project/Details/{model.ProjectId}");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, "The Employee is already assigned to this project.");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong :(");
            }

            model.Employees = await employeeService.GetAllIdAndNameAsync();
            model.Projects = await projectService.GetIdsAndNamesAsync();

            return View(model);
        }
    }
}
