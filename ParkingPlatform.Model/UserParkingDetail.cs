using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model
{
    public class UserParkingDetail
    {
        public int Id { get; set; }
        public string UserId {  get; set; }
        public int ParkingSlotId {  get; set; }
        public string VehicleNumber { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan? DepartureTime { get; set; } 
        public virtual ApplicationUser Users { get; set; } 
        public virtual ParkingSlot ParkingSlots { get; set; }


    }
}
