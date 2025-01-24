using AutoMapper;
using ParkingPlatform.Model;
using ParkingPlatform.Model.DTO.ApplicationUserDtosFolder;

namespace ParkingPlatform.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 

            //application user mapping to application userdto
            CreateMap<ApplicationUser,ApplicationUserDto>();
        }
    }
}
