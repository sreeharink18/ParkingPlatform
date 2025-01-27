using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string VehicleTypeName { get; set; }
        public decimal HourlyCharge {  get; set; }  
        public decimal PenaltyCharge { get; set; } 
        //public  ICollection<Gate> Gates { get; set; }

    }
}
