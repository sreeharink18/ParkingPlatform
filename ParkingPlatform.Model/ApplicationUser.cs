﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string Name {  get; set; }
        public  ICollection<UserParkingDetail> UserParkingDetails { get; set; } 
        public ICollection <WaitingParkingDetail> WaitingParkingDetails { get; set; } 
    }
}
