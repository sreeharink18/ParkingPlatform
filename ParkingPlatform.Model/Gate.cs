using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model
{
    public class Gate
    {
        public int Id { get; set; }
        public string GateName { get; set; }
        public int SlotSize {  get; set; }  
        public int VehicleTypeId {  get; set; } 
        public TimeSpan PenaltyTime { get; set; }
        public virtual VehicleType VehicleType { get; set; }
        public ICollection<ParkingSlot> ParkingSlots { get; set; }    

    }
}
