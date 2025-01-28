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
    public class WaitingParkingDetailsRepository : Repository<WaitingParkingDetail>,IWaitingParkingDetailsRepository
    {
        private ApplicationDbContext _db;
        public WaitingParkingDetailsRepository(ApplicationDbContext db):base(db) {
            _db = db;
        }
        public async Task UpdateAsync(WaitingParkingDetail model)
        {
            _db.WaitingParkingDetails.Update(model);
        }
    }
}
