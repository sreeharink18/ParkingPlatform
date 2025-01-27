using AutoMapper;
using ParkingPlatform.Model;
using ParkingPlatform.Model.DTO.ApplicationUserDtosFolder;
using ParkingPlatform.Model.DTO.GateDtosFolder;
using ParkingPlatform.Model.DTO.VehicleTypeDtosFolder;

namespace ParkingPlatform.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 

            //application user mapping to application userdto
            CreateMap<ApplicationUser,ApplicationUserDto>();
            CreateMap<VehicleType, VehicleTypeAddDto>().ReverseMap();
            CreateMap<Gate,GateAddDto>().ReverseMap();
        }
    }
}
