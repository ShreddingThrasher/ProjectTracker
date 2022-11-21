﻿using Microsoft.AspNetCore.Authorization;
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
