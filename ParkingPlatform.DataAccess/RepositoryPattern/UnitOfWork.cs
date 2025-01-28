using ParkingPlatform.DataAccess.Data;
using ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.DataAccess.RepositoryPattern
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IApplicationUserRepository ApplicationUserRepository { get; private set; }
        public IVehicleRepository VehicleRepository { get; private set; }
        public IGateRepository GateRepository { get; private set; }
        public IParkingSlotRepository ParkingSlotRepository { get; private set; }
        public IUserParkingDetailsRepository UserParkingDetailsRepository { get; private set; }
        public IWaitingParkingDetailsRepository WaitingParkingDetailsRepository{ get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUserRepository = new ApplicationUserRepository(_db);
            VehicleRepository = new VehicleRepository(_db);
            GateRepository = new GateRepository(_db);
            ParkingSlotRepository = new ParkingSlotRepository(_db);
            UserParkingDetailsRepository = new UserParkingDetailsRepository(_db);
            WaitingParkingDetailsRepository = new WaitingParkingDetailsRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
