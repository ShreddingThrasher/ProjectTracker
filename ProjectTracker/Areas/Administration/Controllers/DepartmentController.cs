using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Core.Constants;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Department;
using ProjectTracker.Infrastructure.Data.Entities.Enums;
using System.Runtime.InteropServices;

namespace ProjectTracker.Areas.Administration.Controllers
{
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService departmentService;
        private readonly IEmployeeService employeeService;

        public DepartmentController(
            IDepartmentService _departmentService,
            IEmployeeService _employeeService)
        {
            departmentService = _departmentService;
            employeeService = _employeeService;
        }

        public async Task<IActionResult> Active()
        {
            var model = await departmentService.GetAllAsync();

            return View(model);
        }

        public async Task<IActionResult> Past()
        {
            var model = await departmentService.GetInactiveDepartmentsAsync();

            return View(model);
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
                model.Employees = await departmentService.GetPossibleLeadersAsync();

                return View(model);
            }

            try
            {
                await departmentService.CreateAsync(model);

                return RedirectToAction(nameof(Active));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                model.Employees = await employeeService.GetAllIdAndNameAsync();

                return View(model);
            }
        }

        public async Task<IActionResult> Change(bool? success)
        {
            var model = new ChangeDepartmentViewModel()
            {
                Departments = await departmentService.GetAllIdAndNameAsync()
            };

            ViewBag.Success = success;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid departmentId)
        {
            try
            {
                var model = await departmentService.GetEditDetailsAsync(departmentId);

                model.Employees = await employeeService.GetAllIdAndNameAsync();

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Change), new { success = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditDepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Employees = await departmentService.GetPossibleLeadersAsync();

                return View(model);
            }


            try
            {
                await departmentService.EditAsync(model);

                return RedirectToAction(nameof(Active));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                model.Employees = await departmentService.GetPossibleLeadersAsync();

                return View(model);
            }
            catch (Exception)
            {
                RedirectToAction(nameof(Change), new { success = false });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Close()
        {
            var model = new DeleteDepartmentViewModel()
            {
                Departments = await departmentService.GetAllIdAndNameAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Close(DeleteDepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Departments = await departmentService.GetAllIdAndNameAsync();

                return View(model);
            }

            try
            {
                await departmentService.DeleteAsync(model.Id);

                return RedirectToAction(nameof(Active));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");

                model.Departments = await departmentService.GetAllIdAndNameAsync();

                return View(model);
            }
        }
    }
}
