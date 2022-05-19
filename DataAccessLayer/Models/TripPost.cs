using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelSystem.DataAccessLayer.Models
{
    public class TripPost
    {
        public Guid Id { get; set; }
        [Required][MaxLength(60)]
        public string AgencyName { get; set; }
        [Required][MaxLength(100)]
        public string Title { get; set; }
        public string Details { get; set; }
        [Required][MaxLength(100)]
        public string Date { get; set; }
        [Required][MaxLength(100)]
        public string PostDate { get; set; }
        [Required][MaxLength(200)]
        public string Destination { get; set; }
        [MaxLength(600)]
        public string PhotoPath { get; set; }

        public bool Accepted { get; set; }

        public List<LikedPostTable> Likedby { get; set; }

        public List<DislikedPostTable> Dislikedby { get; set; }

        public List<SavedPostTable> Savedby { get; set; }

        public string OwnerID { get; set; }
        public ApplicationUser Owner { get; set; }
    }
}
