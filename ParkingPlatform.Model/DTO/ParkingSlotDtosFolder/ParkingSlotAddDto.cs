using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingPlatform.Utility;

namespace ParkingPlatform.Model.DTO.ParkingSlotDtosFolder
{
    public class ParkingSlotAddDto
    {
        public int GateId { get; set; }
   
        public int ParkingSlotNumber { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        [EnumDataType(typeof(StaticData.ParkingStatus), ErrorMessage = "Status must be either 'Available' or 'Engaged'.")]
        public string Status { get; set; }
    }
}
