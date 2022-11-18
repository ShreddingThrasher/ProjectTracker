using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Constants;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Department;
using ProjectTracker.Infrastructure.Data.Entities.Enums;

namespace ProjectTracker.Controllers
{
    public class DepartmentsController : BaseController
    {
        private readonly IDepartmentService departmentService;
        private readonly IEmployeeService employeeService;

        public DepartmentsController(
            IDepartmentService _departmentService,
            IEmployeeService _employeeService)
        {
            departmentService = _departmentService;
            employeeService = _employeeService;
        }

        public async Task<IActionResult> All()
        {
            var model = await departmentService.GetAll();

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Create()
        {
            var model = new CreateDepartmentViewModel()
            {
                Employees = await employeeService.GetAllIdAndNameAsync()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Create(CreateDepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Employees = await employeeService.GetAllIdAndNameAsync();

                return View(model);
            }

            try
            {
                await departmentService.CreateAsync(model);

                return RedirectToAction("All", "Departments");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                model.Employees = await employeeService.GetAllIdAndNameAsync();

                return View(model);
            }
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var model = await departmentService.GetDepartmentDetailsAsync(id);

            if(model == null)
            {
                return RedirectToAction(nameof(HomeController.NotFound), "Home");
            }

            ViewBag.Open = model.Tickets.Where(t => t.Status == Status.Open).Count();
            ViewBag.InProgress = model.Tickets.Where(t => t.Status == Status.InProgress).Count();
            ViewBag.Done = model.Tickets.Where(t => t.Status == Status.Done).Count();

            return View(model);
        }
    }
}
