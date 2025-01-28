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
    public class GateRepository:Repository<Gate>,IGateRepository
    {
        private ApplicationDbContext _db;
        public GateRepository(ApplicationDbContext db):base(db)
        {
                _db = db;
        }

        public async Task UpdateAsync(Gate gate)
        {
            try
            {
                var existing_gate = await _db.Gates.FindAsync(gate.Id);
                if (existing_gate != null)
                {
                    _db.Entry(existing_gate).CurrentValues.SetValues(gate);
                }
            }
            catch (Exception ex)
            {
                throw new($"Error updating Gate: {ex.Message}");
            }
        }
    }
}
