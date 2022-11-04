using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Contracts;

namespace ProjectTracker.Controllers
{
    public class EmployeesController : BaseController
    {
        private readonly IEmployeeService employeeService;

        public EmployeesController(IEmployeeService _employeeService)
        {
            employeeService = _employeeService;
        }

        public async Task<IActionResult> All()
        {
            var model = await employeeService.GetAll();

            return View(model);
        }
    }
}
