using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PustokStart.Models;
using PustokStart.ViewModels;

namespace PustokStart.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(MemberLoginViewModel memberVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Email or password wrong");
               return View();
            }
            AppUser user = await _userManager.FindByEmailAsync(memberVM.Email);
            if (user == null || user.IsAdmin)
            {
				ModelState.AddModelError("", "Email or password wrong");
                return View();

			}
            var result = await _signInManager.PasswordSignInAsync(user, memberVM.Password, false, false);
            if (!result.Succeeded)
            {
				ModelState.AddModelError("", "Username or password incorrect");
				return View();
			}
            return RedirectToAction("Index","home");
			;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(MemberRegisterViewModel registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if(await _userManager.Users.AnyAsync(x=>x.UserName==registerVM.UserName)) 
            {
                ModelState.AddModelError("UserName", "UserName is already used");
                return View();
            }
			if (await _userManager.Users.AnyAsync(x => x.Email == registerVM.Email))
			{
				ModelState.AddModelError("Email", "Email is already used");
				return View();
			}
            AppUser user= new AppUser()
            {
                FullName=registerVM.FullName,
                Email=registerVM.Email,
                UserName=registerVM.UserName,
                IsAdmin=false,

            };
            var result =await _userManager.CreateAsync(user,registerVM.Password);
            if (!result.Succeeded) 
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
				return View();
			}

            await _signInManager.SignInAsync(user,false);
            return RedirectToAction("login","account");
           

		}
        public async Task<IActionResult> Logout()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction("login","account");
        }
        


    }
}
