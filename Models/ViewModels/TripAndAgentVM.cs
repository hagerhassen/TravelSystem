using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelSystem.DataAccessLayer.Models;

namespace TravelSystem.Models.ViewModels
{
    public class TripAndAgentVM
    {
        public IEnumerable<ApplicationUser> agentsList { get; set; }
        public TripPost Trip { get; set; }
        public string statusMessage { get; set; }
    }
}
