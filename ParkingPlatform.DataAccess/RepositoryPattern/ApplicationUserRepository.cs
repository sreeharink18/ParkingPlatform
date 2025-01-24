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
    public class ApplicationUserRepository : Repository<ApplicationUser>,IApplicationUserRepository
    {
        private ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) :base(db) { 
         _db = db;
        }

        public async Task UpdateAsync(ApplicationUser model)
        {
            _db.ApplicationUsers.Update(model);
        }
    }
}
