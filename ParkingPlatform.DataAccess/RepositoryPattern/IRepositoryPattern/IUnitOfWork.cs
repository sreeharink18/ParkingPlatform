﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern
{
    public interface IUnitOfWork
    {
         IApplicationUserRepository ApplicationUserRepository { get; }
         IVehicleRepository VehicleRepository { get; }
         IGateRepository GateRepository { get; }
        IParkingSlotRepository ParkingSlotRepository { get; }
        IUserParkingDetailsRepository UserParkingDetailsRepository { get; }
        IWaitingParkingDetailsRepository WaitingParkingDetailsRepository { get; }
        
         void Save();
    }
}
