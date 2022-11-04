using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Contracts;

namespace ProjectTracker.Controllers
{
    public class DepartmentsController : BaseController
    {
        private readonly IDepartmentService departmentService;

        public DepartmentsController(IDepartmentService _departmentService)
        {
            departmentService = _departmentService;
        }

        public async Task<IActionResult> All()
        {
            var model = await departmentService.GetAll();

            return View(model);
        }
    }
}
