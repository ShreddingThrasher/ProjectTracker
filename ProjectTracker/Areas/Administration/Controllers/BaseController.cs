using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectTracker.Core.Constants;
using System.Security.Claims;

namespace ProjectTracker.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = RoleConstants.Admin)]
    public class BaseController : Controller
    {
        public string UserFirstName
        {
            get
            {
                string firstName = string.Empty;

                if (User != null && User.HasClaim(c => c.Type == ClaimTypeConstants.FirstName))
                {
                    firstName = User.Claims
                        .FirstOrDefault(c => c.Type == ClaimTypeConstants.FirstName)
                        ?.Value ?? firstName;
                }

                return firstName;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                ViewBag.UserFirstName = UserFirstName;
            }

            base.OnActionExecuted(context);
        }
    }
}
