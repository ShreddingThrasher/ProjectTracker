using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Contracts;

namespace ProjectTracker.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService _employeeService)
        {
            employeeService = _employeeService;
        }

        public async Task<IActionResult> All()
        {
            var model = await employeeService.GetAllAsync();

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var model = await employeeService.GetEmployeeDetailsAsync(id);

            if (model == null)
            {
                return RedirectToAction(nameof(HomeController.NotFound), "Home");
            }

            return View(model);
        }
    }
}
