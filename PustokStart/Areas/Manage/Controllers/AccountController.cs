using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PustokStart.Areas.Manage.ViewModels;
using PustokStart.Models;

namespace PustokStart.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController:Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser user= new AppUser()
        //    {
        //        UserName="admin",
        //        IsAdmin=true, 

        //    };
        //    var result = await _userManager.CreateAsync(user,"Admin123"); 
        //   return Json(result); 
        //}
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginViewModel adminVM,string returnUrl=null)
        {
           
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();
            }
            AppUser user = await _userManager.FindByNameAsync(adminVM.UserName);
            if (user == null || !user.IsAdmin)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();

            }
            var result = await _signInManager.PasswordSignInAsync(user, adminVM.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();

            }
            if(returnUrl!= null) { return Redirect(returnUrl); };
            return RedirectToAction("index","dashboard");
        }
        public IActionResult ShowUser()
        {
            return Json(new
            {
                isAuthen=User.Identity.IsAuthenticated,
                username=User.Identity.Name,
            });
        }

    }
}
