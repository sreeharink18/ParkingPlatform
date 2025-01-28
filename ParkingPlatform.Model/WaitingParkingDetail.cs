using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model
{
    public class WaitingParkingDetail
    {
        public int Id { get; set; }
        public string UserId {  get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; }
        public string VehicleNumber {  get; set; }  
        public DateTime ArrivalWaitingTime { get; set; }
        public string VehicleType {  get; set; }   

    }
}
