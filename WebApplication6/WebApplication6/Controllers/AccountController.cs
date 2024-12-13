using Demo.Dal.Entities;
using Demo.PL.Utility;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Common;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = new AppUser
            {
                FName = model.FName,
                LName = model.LName,
                Email = model.Email,
                Agree = model.Agree,
                UserName = model.FName + model.LName,
            };
            var Result = await _userManager.CreateAsync(user, model.Password);
            if (Result.Succeeded) return RedirectToAction(nameof(Login));
            foreach (var error in Result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is not null)
            {
                if (await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded) return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Incorrect Email Or Password");
            return View();
        }
        public IActionResult SignOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        public IActionResult ForgetPassword()
        {
          return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM model)
        {
            if(!ModelState.IsValid) return View(model);

           var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is not null) 
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var url = Url.Action("ResetPassword", "Account", new { email = model.Email , token=token},Request.Scheme);
                var email = new Email
                {
                    Recipient = model.Email,
                    Subject = "Reset Password",
                    Body = url
                };
                MailSettings.SendEmail(email);
                return RedirectToAction(nameof(CheckYourInbox));
            }
            ModelState.AddModelError("", "Email Dosen`t Exist");
            return View(); 
        }

        public IActionResult CheckYourInbox() 
        {
            return View(); 
        }

    }
}
 