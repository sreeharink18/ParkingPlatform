using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ParkingPlatform.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Gate> Gates { get; set; }
        public DbSet<ParkingSlot> ParkingSlots { get; set; }
        public DbSet<UserParkingDetail> UserParkingDetails { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<WaitingParkingDetail> WaitingParkingDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<UserParkingDetail>()
            //    .HasOne(upd => upd.Users) 
            //    .WithMany(u => u.UserParkingDetails)
            //    .HasForeignKey(upd => upd.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<UserParkingDetail>()
            //   .HasOne(upd => upd.ParkingSlots) 
            //   .WithMany(ps => ps.UserParkingDetails)  
            //   .HasForeignKey(upd => upd.ParkingSlotId)
            //   .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<ParkingSlot>()
            //    .HasOne(ps => ps.Gates) 
            //    .WithMany(g => g.ParkingSlots) 
            //    .HasForeignKey(ps => ps.GateId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<WaitingParkingDetail>()
            //    .HasOne(wpd => wpd.Users) 
            //    .WithMany(u => u.WaitingParkingDetails) 
            //    .HasForeignKey(wpd => wpd.UserId)
            //    .OnDelete(DeleteBehavior.Cascade); 

            //modelBuilder.Entity<Gate>()
            //    .HasOne(g => g.VehicleTypes) 
            //    .WithMany() 
            //    .HasForeignKey(g => g.VehicleTypeId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserParkingDetail>()
                .HasOne(upd => upd.Users)
                .WithMany(u => u.UserParkingDetails)
                .HasForeignKey(upd => upd.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserParkingDetail>()
                .HasOne(upd => upd.ParkingSlots)
                .WithMany(ps => ps.UserParkingDetails)
                .HasForeignKey(upd => upd.ParkingSlotId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ParkingSlot>()
                .HasOne(ps => ps.Gate)
                .WithMany(g => g.ParkingSlots)
                .HasForeignKey(ps => ps.GateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WaitingParkingDetail>()
                .HasOne(wpd => wpd.Users)
                .WithMany(u => u.WaitingParkingDetails)
                .HasForeignKey(wpd => wpd.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Gate>()
                .HasOne(g => g.VehicleType)
                .WithMany() 
                .HasForeignKey(g => g.VehicleTypeId) 
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
