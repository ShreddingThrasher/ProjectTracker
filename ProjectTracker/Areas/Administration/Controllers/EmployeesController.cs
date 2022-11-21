using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Contracts;

namespace ProjectTracker.Areas.Administration.Controllers
{
    public class EmployeesController : BaseController
    {
        private readonly IEmployeeService employeeService;

        public EmployeesController(IEmployeeService _employeeService)
        {
            employeeService = _employeeService;
        }
    }
}
