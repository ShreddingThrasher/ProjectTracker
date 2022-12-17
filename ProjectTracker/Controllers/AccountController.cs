using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Account;

namespace ProjectTracker.Controllers
{
    public class AccountController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IAccountService accountService;

        public AccountController(
            IAccountService _accountService,
            RoleManager<IdentityRole> _roleManager)
        {
            accountService = _accountService;
            roleManager = _roleManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await accountService.RegisterAsync(model);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if(User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await accountService.LoginAsync(model);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Login failed");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await accountService.LogoutAsync(User);

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Guest()
        {
            var model = new GuestRegisterViewModel()
            {
                Roles = await roleManager.Roles.Select(r => r.Name).ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Guest(GuestRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Something went Wrong");

                model.Roles = await roleManager.Roles.Select(r => r.Name).ToListAsync();

                return View(model);
            }

            var result = await accountService.GuestRegisterAsync(model);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            ModelState.AddModelError(string.Empty, "Something went Wrong");

            model.Roles = await roleManager.Roles.Select(r => r.Name).ToListAsync();

            return View(model);
        }
    }
}
