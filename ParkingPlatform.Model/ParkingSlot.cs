using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model
{
    public class ParkingSlot
    {
        public int Id { get; set; } 
        public int GateId {  get; set; }
        [ForeignKey(nameof(GateId))]
        public Gate Gate { get; set; }
        public int ParkingSlotNumber {  get; set; }
        
        public string Status { get; set; }
  

    }
}
