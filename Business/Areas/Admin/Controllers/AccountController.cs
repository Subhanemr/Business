using Business.Areas.Admin.ViewModels;
using Business.Models;
using Business.Utilities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Business.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string? returnUrl, LoginVM login)
        {
            if (!ModelState.IsValid) return View(login);
            AppUser user = await _userManager.FindByNameAsync(login.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(login.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Username, Email or Password wrong");
                    return View(login);
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user, login.Password, false, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Is locked out");
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Username, Email or Password wrong");
                return View(login);
            }
            if (returnUrl != null) return Redirect(returnUrl);
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "Home", new { Area = "" });
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View(register);
            AppUser user = new AppUser
            {
                Name = register.Name,
                Surname = register.Surname,
                Email = register.Email,
                UserName = register.UserName
            };
            var result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(register);
            }
            await _userManager.AddToRoleAsync(user, UserRoles.Member.ToString());
            return RedirectToAction(nameof(Login));
        }
    }
}
