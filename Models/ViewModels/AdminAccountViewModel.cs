using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelSystem.Models.ViewModels
{
    public class AdminAccountViewModel
    {
        public string UserID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required,Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required,Display(Name ="Last Name")]
        public string LastName { get; set; }
        [DataType(DataType.PhoneNumber),Display(Name ="Phone Number")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
        public string CurrentPhotoPath { get; set; }
        [Display(Name ="Update Profile Photo")]
        public IFormFile Photo { get; set; }
        [DataType(DataType.Password), Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }
        [DataType(DataType.Password),Display(Name ="Update Password")]
        public string UpdatePassword { get; set; }
        [Compare("UpdatePassword"),Display(Name ="Confirm Password"),DataType(DataType.Password)]
        public string confirmUpdatedPassword { get; set; }
        public string ErrorInPassword { get; set; }
    }
}
