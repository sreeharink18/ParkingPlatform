using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model.DTO.VehicleTypeDtosFolder
{
    public class VehicleTypeAddDto
    {
        public string VehicleTypeName { get; set; }
        public decimal HourlyCharge { get; set; }
        public decimal PenaltyCharge { get; set; }
    }
}
