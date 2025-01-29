using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model.DTO.VehicleTypeDtosFolder
{
    public class VehicleTypeAddDto
    {
        [RegularExpression(@"^(Two Wheeler|Four Wheeler)$", ErrorMessage = "Vechile Type must be either 'Two Wheeler' or 'Four Wheeler'.")]
        public string VehicleTypeName { get; set; }
        public decimal HourlyCharge { get; set; }
        public decimal PenaltyCharge { get; set; }
    }
}
