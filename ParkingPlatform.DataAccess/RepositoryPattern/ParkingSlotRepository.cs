using ParkingPlatform.DataAccess.Data;
using ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern;
using ParkingPlatform.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.DataAccess.RepositoryPattern
{
    public class ParkingSlotRepository : Repository<ParkingSlot>, IParkingSlotRepository
    {
        private ApplicationDbContext _db;
        public ParkingSlotRepository(ApplicationDbContext db):base(db) { 
            _db = db;
        }
        public async Task UpdateAsync(ParkingSlot parkingSlot)
        {
            _db.ParkingSlots.Update(parkingSlot);
        }
    }
}
