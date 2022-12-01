using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Core.Constants;
using ProjectTracker.Core.ViewModels.Account;
using ProjectTracker.Infrastructure.Data.Entities;

namespace ProjectTracker.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<Employee> userManager;
        private readonly SignInManager<Employee> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(
            UserManager<Employee> _userManager,
            SignInManager<Employee> _signInManager,
            RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
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

            var user = new Employee()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(
                    user, new System.Security.Claims.Claim(ClaimTypeConstants.FirstName, user.FirstName));

                await signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
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

            var user = await userManager.FindByEmailAsync(model.Email);

            if(user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Login failed");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {

            await signInManager.SignOutAsync();

            var user = await userManager.FindByNameAsync(User.Identity.Name);

            if (user.IsGuest)
            {
                await userManager.DeleteAsync(user);
            }

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

            var rnd = new Random();

            int guestRnd = rnd.Next(1, int.MaxValue);

            var guest = new Employee()
            {
                Email = $"guest{guestRnd}@mail.com",
                FirstName = "Guest",
                LastName = "Guest",
                UserName = $"Guest{guestRnd}",
                IsGuest = true
            };

            var roles = await roleManager.Roles.Select(r => r.Name).ToListAsync();

            if(!roles.Contains(model.Role))
            {
                if(model.Role != "Regular")
                {
                    ModelState.AddModelError(string.Empty, "Something went Wrong");

                    model.Roles = await roleManager.Roles.Select(r => r.Name).ToListAsync();

                    return View(model);
                }
            }

            var result = await userManager.CreateAsync(guest);

            if (result.Succeeded)
            {

                await userManager.AddClaimAsync(
                    guest, new System.Security.Claims.Claim(ClaimTypeConstants.FirstName, guest.FirstName));

                if (model.Role == "Regular")
                {
                    await signInManager.SignInAsync(guest, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var roleResult = await userManager.AddToRoleAsync(guest, model.Role);

                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(guest, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Something went Wrong");

            model.Roles = await roleManager.Roles.Select(r => r.Name).ToListAsync();

            return View(model);
        }
    }
}
