using Appointement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Appointement.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected ApplicationUser _currentUser;
        public BaseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _currentUser = await _userManager.GetUserAsync(User);
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
