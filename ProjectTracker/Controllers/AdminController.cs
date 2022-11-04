using Microsoft.AspNetCore.Authorization;
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

        public AdminController(
            IAdminService _adminService,
            IEmployeeService _employeeService,
            RoleManager<IdentityRole> roleManager)
        {
            adminService = _adminService;
            employeeService = _employeeService;
            this.roleManager = roleManager;
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
                Roles = await adminService.GetAllRoles(),
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
    }
}
