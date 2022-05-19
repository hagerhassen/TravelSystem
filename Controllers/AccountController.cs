using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelSystem.DataAccessLayer.Controller;
using TravelSystem.DataAccessLayer.Models;
using TravelSystem.Models.ViewModels;

namespace TravelSystem.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly IDataControl dataControl;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(IDataControl dataControl,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            this.dataControl = dataControl;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("Login"),HttpPost]
        public async Task<IActionResult> login(LoginViewModel model)
        {
            var user =await userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result=await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe,false);
                if (result.Succeeded)
                {
                    if (await userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }
                else {
                    return RedirectToAction("Index", "Home",new { LoginError="Invalid Login Information" });
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Route("Register"),HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ApplicationUser user = new()
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            if(model.Role== "Traveler")
            {
                var result = await dataControl.CreateTraveler(user, model.Password);
                if (!result.Succeeded)
                {
                    string Errors = "";
                    foreach(var error in result.Errors)
                    {
                        Errors += error.Description;
                    }
                    return RedirectToAction("Index", "Home",new { SignUpError = Errors });
                }
            }else if (model.Role == "Agency")
            {
                var result = await dataControl.CreateAgency(user, model.Password);
                if (!result.Succeeded)
                {
                    string Errors = "";
                    foreach (var error in result.Errors)
                    {
                        Errors += error.Description;
                    }
                    return RedirectToAction("Index", "Home", new { SignUpError = Errors });
                }
            }
            await signInManager.SignInAsync(user, model.RememberMe);
            return RedirectToAction("Index", "Home");
        }
    }
}
