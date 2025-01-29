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
        [Required(ErrorMessage = "Gate Name is required.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Gate Name must contain only alphabets.")]
        public string GateName { get; set; }

        [Required(ErrorMessage = "Slot Size is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Slot Size must be greater than 0.")]
        public int SlotSize { get; set; }

        [Required(ErrorMessage = "Vehicle Type ID is required.")]
        public int VehicleTypeId { get; set; }

        [Required(ErrorMessage = "Penalty Time is required.")]
        [RegularExpression(@"^(?:[01]\d|2[0-3]):[0-5]\d:[0-5]\d$", ErrorMessage = "Penalty Time must be in 'HH:mm:ss' format.")]
        public string PenaltyTime { get; set; }
    }
}
