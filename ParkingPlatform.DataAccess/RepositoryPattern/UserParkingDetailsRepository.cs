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
    public class UserParkingDetailsRepository : Repository<UserParkingDetail>,IUserParkingDetailsRepository
    {
        private ApplicationDbContext _db;
        public UserParkingDetailsRepository(ApplicationDbContext db):base(db) { 
            _db = db;
        }

        public async Task Update(UserParkingDetail userParkingDetail)
        {
            _db.UserParkingDetails.Update(userParkingDetail);
        }
    }
}
