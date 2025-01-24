using ParkingPlatform.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        Task UpdateAsync(ApplicationUser model);
    }
}
