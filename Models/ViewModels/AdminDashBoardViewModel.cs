using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelSystem.Models.ViewModels
{
    public class AdminDashBoardViewModel
    {
        public int TravelersNumber { get; set; }
        public int AgenciesNumber { get; set; }
        public int PostsNumber { get; set; }
        public int AcceptedPostsNumber { get; set; }
    }
}
