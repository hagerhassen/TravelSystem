using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelSystem.DataAccessLayer.Models;

namespace TravelSystem.DataAccessLayer.Controller
{
    public interface IDataControl
    {
        public Task<IdentityResult> CreateTraveler(ApplicationUser user, string password);

        public Task<IdentityResult> CreateAgency(ApplicationUser user, string password);

        public Task<IdentityResult> DeleteUser(ApplicationUser user);

        public void DeleteTrip(Guid Id);

        public Task AddTrip(string userID, TripPost post);

        public Task DisLikeTripPost(string userName, Guid postID);

       public Task LikeTripPost(string userName, Guid postID);

        public Task SaveTripPost(string userName, Guid postID);

    }
}
