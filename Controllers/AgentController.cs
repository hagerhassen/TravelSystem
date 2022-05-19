using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelSystem.DataAccessLayer.Database;
using TravelSystem.DataAccessLayer.Models;
using TravelSystem.Models;

namespace TravelSystem.Areas.TripAgency.Controllers
{
    //[Authorize(Roles = SD.AgentUser)]
    [Route("Agent"), Authorize(Roles ="Agency")]
    public class AgentController : Controller
    {
        private readonly AppDBContext db;

        public AgentController(AppDBContext db)
        {
            this.db = db;
        }


        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // ############################## List Agent's Profile Requirments ############################## //
        
        //Get
        [HttpGet][Route("List")]
        public async Task<IActionResult> List()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;

            Trace.WriteLine("USERID : " + userId);

            return View(await db.Users.Where(m => m.Id == userId).ToListAsync());
        }

        // ############################## Edit Agent Profile Requirments ############################## //

        //Get
        [HttpGet][Route("Edit")]
        public async Task<IActionResult> Edit()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;
            if (userId == null)
                return NotFound();
            //Trace.WriteLine("USERID : " + userId);
            var agent = await db.Users.FindAsync(userId);
            if (agent == null)
                return NotFound();
            

            return View(agent);
        }

        //Post
        [HttpPost][Route("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser agent)
        {
            if (ModelState.IsValid)
            {
                db.Users.Update(agent);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(agent);
        }

    }
}
