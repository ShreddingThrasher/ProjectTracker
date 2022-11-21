using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Constants;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Department;
using ProjectTracker.Infrastructure.Data.Entities.Enums;

namespace ProjectTracker.Areas.Administration.Controllers
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

                return Redirect("/Departments/All");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                model.Employees = await employeeService.GetAllIdAndNameAsync();

                return View(model);
            }
        }
    }
}
