using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelSystem.DataAccessLayer.Models;

namespace TravelSystem.Models.ViewModels
{
    public class AdminUsersViewModel
    {
        public List<ApplicationUser> Users { get; set; }
        public List<ApplicationUser> Agencies { get; set; }
    }
}
