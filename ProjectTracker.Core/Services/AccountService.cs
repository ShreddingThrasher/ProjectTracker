using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Core.Constants;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Account;
using ProjectTracker.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<Employee> userManager;
        private readonly SignInManager<Employee> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmployeeService employeeService;

        public AccountService(
            UserManager<Employee> _userManager,
            SignInManager<Employee> _signInManager,
            RoleManager<IdentityRole> _roleManager,
            IEmployeeService _employeeService)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            employeeService = _employeeService;
        }


        /// <summary>
        /// Registers new Guest user
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Result, indicating wether the operation succeeded or no</returns>
        public async Task<IdentityResult> GuestRegisterAsync(GuestRegisterViewModel model)
        {
            var rnd = new Random();

            int guestRnd = rnd.Next(1, 10000);

            var guest = new Employee()
            {
                Email = $"guest{guestRnd}@mail.com",
                FirstName = "Guest",
                LastName = "Guest",
                UserName = $"Guest{guestRnd}",
                IsActive= true,
                IsGuest = true
            };

            if (!roleManager.Roles.Select(r => r.Name).Contains(model.Role))
            {
                if(model.Role != "Regular")
                {
                    return IdentityResult.Failed();
                }
            }

            string password = "Guestttt123!";

            var result = await userManager.CreateAsync(guest, password);

            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(
                    guest, new Claim(ClaimTypeConstants.FirstName, guest.FirstName));

                if(model.Role == "Regular")
                {
                    await signInManager.SignInAsync(guest, true);

                    return result;
                }

                var roleResult = await userManager.AddToRoleAsync(guest, model.Role);

                if(roleResult.Succeeded)
                {
                    await signInManager.SignInAsync(guest, true);

                    return result;
                }

                //in case AddToRoleAsync fails
                await userManager.DeleteAsync(guest);

                return IdentityResult.Failed();
            }

            return result;
        }


        /// <summary>
        /// Logs In existing user
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Result, indicating wether the operation succeeded or no</returns>
        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if(user == null)
            {
                return SignInResult.Failed;
            }

            var result = await signInManager
                .PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return result;
            }

            return SignInResult.Failed;
        }


        /// <summary>
        /// Logs Out the current user.
        /// </summary>
        /// <param name="user">User</param>
        /// <returns></returns>
        public async Task LogoutAsync(ClaimsPrincipal user)
        {
            var employee = await userManager.FindByNameAsync(user?.Identity?.Name);

            await signInManager.SignOutAsync();

            if (employee.IsGuest)
            {
                await employeeService.RemoveById(employee.Id);
            }
        }


        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Result, indicating wether the operation succeeded or no</returns>
        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            var user = new Employee()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(
                    user, new System.Security.Claims.Claim(ClaimTypeConstants.FirstName, user.FirstName));

                await signInManager.SignInAsync(user, false);

                return IdentityResult.Success;
            }
            else
            {
                return result;
            }
        }
    }
}
