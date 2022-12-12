using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Infrastructure.Data.Entities;
using ProjectTracker.Infrastructure.Data.Entities.Enums;
using ProjectTracker.Models;
using System.Diagnostics;

namespace ProjectTracker.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDepartmentService departmentService;
        private readonly IEmployeeService employeeService;
        private readonly IProjectService projectService;
        private readonly ITicketService ticketService;

        public HomeController(
            ILogger<HomeController> logger,
            IDepartmentService _departmentService,
            IEmployeeService _employeeService,
            IProjectService _projectService,
            ITicketService _ticketService)
        {
            _logger = logger;
            departmentService = _departmentService;
            employeeService = _employeeService;
            projectService = _projectService;
            ticketService = _ticketService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Departments"] = await departmentService.GetCountAsync();
            ViewData["Employees"] = await employeeService.GetCountAsync();
            ViewData["Projects"] = await projectService.GetCountAsync();
            ViewData["Tickets"] = await ticketService.GetCountAsync();

            var statuses = await ticketService.GetAllStatusesAsync();

            ViewBag.Open = statuses.Where(s => s == Status.Open).Count();
            ViewBag.InProgress = statuses.Where(s => s == Status.InProgress).Count();
            ViewBag.Done = statuses.Where(s => s == Status.Done).Count();

            return View();
        }

        public new IActionResult NotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}