using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelSystem.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required,EmailAddress,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required,Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [DataType(DataType.Password),Required]
        public string Password { get; set; }
        [Compare("Password"),DataType(DataType.Password),Display(Name ="Confirm Password"),Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Role { get; set; }
        public bool RememberMe { get; set; }
    }
}
