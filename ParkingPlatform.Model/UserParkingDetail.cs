using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model
{
    public class UserParkingDetail
    {
        public int Id { get; set; }
        public string UserId {  get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; }
        public int ParkingSlotId {  get; set; }
        [ForeignKey(nameof(ParkingSlotId))]
        public ParkingSlot ParkingSlot { get; set; }
        public string VehicleNumber { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime? DepartureTime { get; set; } 
        

    }
}
