using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TravelSystem.DataAccessLayer.Controller;
using TravelSystem.DataAccessLayer.Database;
using TravelSystem.DataAccessLayer.Models;
using TravelSystem.Models.ViewModels;

namespace TravelSystem.Controllers
{
    [Route("Admin"),Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDataControl dataControl;
        private readonly AppDBContext appDBContext;
        private readonly IWebHostEnvironment env;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AdminController(UserManager<ApplicationUser> userManager,IDataControl dataControl,AppDBContext appDBContext,IWebHostEnvironment env,SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.dataControl = dataControl;
            this.appDBContext = appDBContext;
            this.env = env;
            this.signInManager = signInManager;
        }


        public async Task<IActionResult> Index()
        {
            var Travelers = await userManager.GetUsersInRoleAsync("Traveler");
            var Agencies = await userManager.GetUsersInRoleAsync("Agency");
            AdminDashBoardViewModel model = new()
            {
                TravelersNumber= Travelers.Count,
                AgenciesNumber=Agencies.Count,
                PostsNumber=appDBContext.TripPosts.Count(),
                AcceptedPostsNumber = appDBContext.TripPosts.Where(e=>e.Accepted==true).Count()
            };
            return View(model);
        }


        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("Account"),HttpGet]
        public async Task<IActionResult> Account()
        {
            ApplicationUser admin = await userManager.FindByNameAsync("Admin");
            AdminAccountViewModel AdminView = new()
            {
                UserID=admin.Id,
                UserName = admin.UserName,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                PhoneNumber = admin.PhoneNumber,
                CurrentPhotoPath = admin.PhotoPath
            };
            return View(AdminView);
        }

        [Route("Accout"),HttpPost]
        public async Task<IActionResult> UpdateAccount(AdminAccountViewModel admin)
        {
            if (!ModelState.IsValid)
            {
                return View("./Account", admin);
            }
            ApplicationUser adminInDB = await userManager.FindByIdAsync(admin.UserID);
            if (admin.UpdatePassword != null && admin.CurrentPassword != null)
            {
                var result = await userManager.ChangePasswordAsync(adminInDB, admin.CurrentPassword, admin.UpdatePassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        admin.ErrorInPassword += error.Description + "";
                    }
                    return View("./Account", admin);
                }
            }
            string PhotoPath = processingUploadedImages(admin);
            UpdateValues(admin, adminInDB, PhotoPath);
            await userManager.UpdateAsync(adminInDB);
            return RedirectToAction("Account");
        }

        private void UpdateValues(AdminAccountViewModel admin, ApplicationUser adminInDB, string PhotoPath)
        {
            adminInDB.PhoneNumber = admin.PhoneNumber;
            if (PhotoPath != null)
            {
                if (adminInDB.PhotoPath != null)
                {
                    DeleteUserImage(adminInDB);
                }
                adminInDB.PhotoPath = PhotoPath;
            }
            adminInDB.FirstName = admin.FirstName;
            adminInDB.LastName = admin.LastName;
        }

        private string processingUploadedImages(AdminAccountViewModel model)
        {
            string photoPath = null;
            if (model.Photo != null)
            {
                string uploadFolder = Path.Combine(env.WebRootPath, "Images", "Avatars");
                photoPath = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                using (FileStream file = new FileStream(Path.Combine(uploadFolder, photoPath), FileMode.Create))
                {
                    model.Photo.CopyTo(file);
                }

            }
            return photoPath;
        }

        [Route("DeleteProfile")]
        public async Task<IActionResult> DeleteProfilePic(string UserID)
        {
            var user = await userManager.FindByIdAsync(UserID);
            if (user.PhotoPath == null)
            {
                return RedirectToAction("Account");
            }
            DeleteUserImage(user);
            user.PhotoPath = null;
            await userManager.UpdateAsync(user);
            return RedirectToAction("Account");
        }

        private void DeleteUserImage(ApplicationUser user)
        {
            FileInfo Photo = new(Path.Combine(env.WebRootPath, "Images", "Avatars", user.PhotoPath));
            Photo.Delete();
        }

        [Route("Users")]
        public async Task<IActionResult> Users()
        {
            AdminUsersViewModel model = new()
            {
                Users = (List<ApplicationUser>)await userManager.GetUsersInRoleAsync("Traveler"),
                Agencies = (List<ApplicationUser>)await userManager.GetUsersInRoleAsync("Agency")
            };
            return View(model);
        }

        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string UserID)
        {
            var user =await userManager.FindByIdAsync(UserID);
            if (user != null)
            {
                await dataControl.DeleteUser(user);
            }
            return RedirectToAction("Users");
        }

        [Route("Posts")]
        public IActionResult Posts()
        {
            var Posts= GetAllPosts();
            return View(Posts);
        }

        [Route("Posts"),HttpPost]
        public IActionResult Posts(TripPost post)
        {
            var postInDB = appDBContext.TripPosts.FirstOrDefault(p => p.Id == post.Id);
            UpdatePost(post, postInDB);
            var UpdatedPost = appDBContext.TripPosts.Attach(postInDB);
            UpdatedPost.State = EntityState.Modified;
            appDBContext.SaveChanges();
            return RedirectToAction("Posts");
        }

        [Route("DeletePost")]
        public IActionResult DeletePost(Guid PostID)
        {
            var post = appDBContext.TripPosts.FirstOrDefault(p => p.Id == PostID);
            if (post != null)
            {
                appDBContext.TripPosts.Remove(post);
                appDBContext.SaveChanges();
            }
            return RedirectToAction("Posts");
        }

        [Route("ApprovePost")]
        public IActionResult ApprovePost(Guid PostID)
        {
            var post = appDBContext.TripPosts.FirstOrDefault(p => p.Id == PostID);
            if (post != null)
            {
                post.Accepted = true;
                var UpdatedPost = appDBContext.TripPosts.Attach(post);
                UpdatedPost.State = EntityState.Modified;
                appDBContext.SaveChanges();
            }
            return RedirectToAction("Posts");
        }

        private List<TripPost> GetAllPosts()
        {
            return appDBContext.TripPosts
                            .Include(e => e.Owner)
                            .Include(e => e.Likedby)
                            .Include(e => e.Dislikedby)
                            .ToList();
        }

        private static void UpdatePost(TripPost post, TripPost postInDB)
        {
            postInDB.Title = post.Title;
            postInDB.Destination = post.Destination;
            postInDB.Details = post.Details;
        }
    }
}
