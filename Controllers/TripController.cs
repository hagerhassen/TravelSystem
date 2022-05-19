using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelSystem.DataAccessLayer.Database;
using TravelSystem.DataAccessLayer.Models;
using TravelSystem.Models.ViewModels;

namespace TripAgencyModule.Areas.TripAgency.Controllers
{
    [Route("Trip"),Authorize(Roles ="Agency")]
    public class TripController : Controller
    {
        [TempData]
        public string StatusMessage { get; set; }

        private readonly AppDBContext db;


        public TripController(AppDBContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // ############################## List Agent' Posts Requirments ############################## //

        //Get
        [HttpGet] [Route("List")]
        public async Task<IActionResult> List()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;

            return View(await db.TripPosts.Where(m => m.OwnerID == userId).ToListAsync());
        }

        // ############################## Create New Post Requirments ############################## //

        //Get
        [HttpGet] [Route("Create")]
        public async Task<IActionResult> Create()
        {
            TripAndAgentVM model = new TripAndAgentVM()
            {
                agentsList = await db.Users.ToListAsync(),
                Trip = new TripPost()
            };

            return View(model);
        }

        //Post
        [HttpPost] [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TripAndAgentVM model)
        {
            if (ModelState.IsValid)
            {

                var isExistTrip = await db.TripPosts.Include(m => m.Owner)
                    .Where(m => m.Owner.Id == model.Trip.OwnerID && m.AgencyName == model.Trip.AgencyName).ToListAsync();
                if (isExistTrip.Count() > 0)
                    StatusMessage = "Error : You Created A Trip With The Same Name Before";
                else
                {
                    //string image = uploadImages();
                    //model.Trip.Image = image;
                    model.Trip.OwnerID = getCurrUserId();
                    db.TripPosts.Add(model.Trip);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(List));
                }
            }

            TripAndAgentVM viewModel = new TripAndAgentVM()
            {
                agentsList = await db.Users.ToListAsync(),
                Trip = model.Trip,
                statusMessage = StatusMessage
            };

            return View(viewModel);
        }
        private string getCurrUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;
            return userId;
        }

        [HttpGet][Route("GetTripsRelatedToAgent")]
        public async Task<IActionResult> GetTripsRelatedToAgent(string? id)
        {
            List<TripPost> trips = new List<TripPost>();
            trips = await db.TripPosts.Where(m => m.OwnerID == id).ToListAsync();
            return Json(new SelectList(trips, "Id", "Name"));
        }


        // ############################## Edit Exist Post Requirments ############################## //

        //Get
        [HttpGet][Route("Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var trip = await db.TripPosts.FindAsync(id);
            if (trip == null)
                return NotFound();

            TripAndAgentVM viewModel = new TripAndAgentVM()
            {
                agentsList = await db.Users.ToListAsync(),
                Trip = trip
            };

            return View(viewModel);
        }

        //Post
        [HttpPost][Route("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TripAndAgentVM model)
        {
            if (ModelState.IsValid)
            {
                var isExistTrip = await db.TripPosts.Include(m => m.Owner)
                    .Where(m => m.Owner.Id == model.Trip.OwnerID &&
                           m.AgencyName == model.Trip.AgencyName && m.Id != model.Trip.Id).ToListAsync();
                if (isExistTrip.Count() > 0)
                {
                    StatusMessage = "Error : You Created A Trip With The Same Name Before";
                }
                else
                {
                    db.TripPosts.Update(model.Trip);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(List));
                }
            }

            TripAndAgentVM modelVM = new TripAndAgentVM()
            {
                agentsList = await db.Users.ToListAsync(),
                Trip = model.Trip,
                statusMessage = StatusMessage
            };

            return View(modelVM);
        }

        // ############################## Delete Exist Post Requirments ############################## //

        //Get
        [HttpGet][Route("Delete")]
        public IActionResult Delete(Guid id)
        {
            var trip = db.TripPosts.Include(m => m.Owner).Where(m => m.Id == id).SingleOrDefault();
            if (trip == null)
                return NotFound();

            return View(trip);
        }

        //Post
        [HttpPost][Route("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TripPost trip)
        {
            db.TripPosts.Remove(trip);
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

    }
}
