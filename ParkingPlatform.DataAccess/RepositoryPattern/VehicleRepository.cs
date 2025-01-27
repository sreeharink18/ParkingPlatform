using ParkingPlatform.DataAccess.Data;
using ParkingPlatform.DataAccess.RepositoryPattern;
using ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern;
using ParkingPlatform.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.DataAccess.RepositoryPattern
{
    public class VehicleRepository : Repository<VehicleType>,IVehicleRepository
    {
        private ApplicationDbContext _db;

        public VehicleRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(VehicleType vehicle)
        {
            try
            {
                var existingVehicle = await _db.VehicleTypes.FindAsync(vehicle.Id);  
                if (existingVehicle != null)
                {
                    _db.Entry(existingVehicle).CurrentValues.SetValues(vehicle);
                    //_db.VehicleTypes.Update(vehicle);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating vehicle: {ex.Message}");
            }
        }

    }
}
