using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model.DTO.ApplicationUserDtosFolder
{
    public class UserRegisterRequestDto
    {
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password Length must be 8")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }
       
    }
    
}
