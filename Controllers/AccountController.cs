using Appointement.Models;
using Appointement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Appointement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var result = await _userManager.FindByEmailAsync(user.Email);

            if(result is not null)
            {
                var signinResult = await _signInManager.PasswordSignInAsync(result, user.Password, user.Remberme, false);
                if (signinResult.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                    ModelState.AddModelError(string.Empty, "Invalid Password.");
            }
            else
                ModelState.AddModelError(string.Empty, "Invalid Email.");


            return View(user);
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewModel user)
        {
            if(!ModelState.IsValid)
                return View(user);
            var appUser = new ApplicationUser
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.Email.Split('@')[0]
            };

            var result = await _userManager.CreateAsync(appUser, user.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(appUser, false);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            // get the user
            var user = await _userManager.GetUserAsync(User);
            return View("EditUserEmail", new UpdateUserEmailViewModel {  Email = user.Email, UserName = user.UserName, PhoneNumber = user.PhoneNumber });
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetUserInfo(UpdateUserEmailViewModel model)
        {
            if(!ModelState.IsValid)
                return View("EditUserEmail", model);

            // get the applicationuser object
            var user = await _userManager.GetUserAsync(User);


            // generate email token
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
            var result = await _userManager.ChangeEmailAsync(user, model.Email, token);
            if (!result.Succeeded)
            {
                return View("EditUserEmail", model);
            }

            // change username & phone number
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return View("EditUserEmail", model);
            }

            await _signInManager.RefreshSignInAsync(user);
            return View("EditUserEmail", model);
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if(result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}
