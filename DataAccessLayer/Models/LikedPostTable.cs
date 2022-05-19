using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelSystem.DataAccessLayer.Models
{
    public class LikedPostTable
    {
        public string userID { get; set; }
        public ApplicationUser user { get; set; }

        public Guid postID { get; set; }
        public TripPost post { get; set; }
    }
}
