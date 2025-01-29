using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model.DTO.UserParkingDetailDtosFolder
{
    public class UserParkingDetailResponseDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Arrival {  get; set; }
        public DateTime Depature { get; set; }
        public TimeSpan TotalTimeSpend { get; set; }
        public decimal PenaltyCharge { get; set; }
        public decimal HourlyCharge { get; set; }
        public decimal TotalAmount { get; set; }
        public string GateName { get; set; }
        public int ParkingSlotNumber { get; set; }
        public string VehicleNumber { get; set; }

    }
}
