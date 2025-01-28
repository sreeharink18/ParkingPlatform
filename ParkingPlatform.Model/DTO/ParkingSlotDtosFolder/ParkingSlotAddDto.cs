using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model.DTO.ParkingSlotDtosFolder
{
    public class ParkingSlotAddDto
    {
        public int GateId { get; set; }
   
        public int ParkingSlotNumber { get; set; }

        public string Status { get; set; }
    }
}
