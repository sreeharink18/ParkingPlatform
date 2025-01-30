using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParkingPlatform.DataAccess.Data;
using ParkingPlatform.Model;
using ParkingPlatform.Utility;

namespace ParkingPlatform.DbInitializeService
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public DbInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        public void Initialize()
        {
            //migration if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }
            //crate role if they are not created
            if (!_roleManager.RoleExistsAsync(StaticData.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(StaticData.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticData.Role_Admin)).GetAwaiter().GetResult();
               

                //if roles are not created , then we will created admin user as well
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    Name = "MasterAdmin",
                    PhoneNumber = "1234567890",
                  

                }, "Admin@123").GetAwaiter().GetResult();
                ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(applicationUser, StaticData.Role_Admin).GetAwaiter().GetResult();

            }
            return;


        }
    }
}
