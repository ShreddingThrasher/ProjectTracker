using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Contracts;

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
    }
}
