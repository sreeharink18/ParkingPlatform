using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model
{
    public class ParkingSlot
    {
        public int Id { get; set; } 
        public int GateId {  get; set; }
        public int ParkingSlotNumber {  get; set; } 
        public string Status { get; set; }
        public ICollection<UserParkingDetail> UserParkingDetails { get; set; }
        public virtual Gate Gate { get; set; }

    }
}
