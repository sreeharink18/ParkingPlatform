using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model.DTO.UserParkingDetailDtosFolder
{
    public class CreateUserParkingDetailRequestDto
    {

        public string UserId { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleType { get; set; }

    }
}
