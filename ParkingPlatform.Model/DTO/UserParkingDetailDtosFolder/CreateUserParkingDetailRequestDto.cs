using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model.DTO.UserParkingDetailDtosFolder
{
    public class CreateUserParkingDetailRequestDto
    {
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }
        [Required(ErrorMessage ="Vechile Number is required")]
        public string VehicleNumber { get; set; }
        [RegularExpression(@"^(Two Wheeler|Four Wheeler)$", ErrorMessage = "Vechile Type must be either 'Two Wheeler' or 'Four Wheeler'.")]
        public string VehicleType { get; set; }

    }
}
