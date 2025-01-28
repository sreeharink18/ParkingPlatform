using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model.DTO.GateDtosFolder
{
    public class GateAddDto
    {
        [Required]
        public string GateName { get; set; }
        [Required]
        public int SlotSize { get; set; }
        [Required]
        public int VehicleTypeId { get; set; }
        [Required]
        public string PenaltyTime { get; set; }
    }
}
