using Microsoft.AspNetCore.Identity;
using ProjectTracker.Core.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Contracts
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task<IdentityResult> GuestRegisterAsync(GuestRegisterViewModel model);

        Task LogoutAsync(ClaimsPrincipal user);
    }
}
