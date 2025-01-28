using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model
{
    public class WaitingParkingDetail
    {
        public int Id { get; set; }
        public string UserId {  get; set; }
        public string VehicleNumber {  get; set; }  
        public TimeSpan ArrivalWaitingTime { get; set; }
        public string VehicleType {  get; set; }   
        public virtual ApplicationUser Users { get; set; } 
    }
}
