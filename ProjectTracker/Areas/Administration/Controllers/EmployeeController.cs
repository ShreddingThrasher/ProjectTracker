using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Employee;

namespace ProjectTracker.Areas.Administration.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService _employeeService)
        {
            employeeService = _employeeService;
        }

        public async Task<IActionResult> Active()
        {
            var model = await employeeService.GetActiveAsync();

            return View(model);
        }

        public async Task<IActionResult> Unassigned()
        {
            var model = await employeeService.GetUnassignedAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(bool? success)
        {
            var model = new RemoveEmployeeViewModel()
            {
                Employees = await employeeService.GetAllIdAndNameAsync()
            };

            ViewBag.Success = success;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(RemoveEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Employees = await employeeService.GetAllIdAndNameAsync();

                return View(model);
            }

            await employeeService.RemoveById(model.Id);

            return RedirectToAction(nameof(Active));
        }
    }
}
