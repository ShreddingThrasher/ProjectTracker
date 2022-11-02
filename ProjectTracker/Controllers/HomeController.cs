﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Infrastructure.Data.Entities;
using ProjectTracker.Models;
using System.Diagnostics;

namespace ProjectTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
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
            ViewData["Departments"] = await departmentService.GetCount();
            ViewData["Employees"] = await employeeService.GetCount();
            ViewData["Projects"] = await projectService.GetCount();
            ViewData["Tickets"] = await ticketService.GetCount();

            return View();
        }

        public IActionResult Dashboard()
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