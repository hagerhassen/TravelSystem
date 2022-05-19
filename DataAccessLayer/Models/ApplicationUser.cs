using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelSystem.DataAccessLayer.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required][MaxLength(30)]
        public string FirstName { get; set; }
        [Required][MaxLength(30)]
        public string LastName { get; set; }
        public string PhotoPath { get; set; }

        public List<TripPost> Posted { get; set; } = new();
        public List<LikedPostTable> LikedPosts { get; set; } = new();
        public List<DislikedPostTable> DislikedPosts { get; set; } = new();
        public List<SavedPostTable> SavedPosts { get; set; } = new();
    }
}
