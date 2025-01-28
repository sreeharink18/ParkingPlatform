using AutoMapper;
using ParkingPlatform.Model;
using ParkingPlatform.Model.DTO.ApplicationUserDtosFolder;
using ParkingPlatform.Model.DTO.GateDtosFolder;
using ParkingPlatform.Model.DTO.ParkingSlotDtosFolder;
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
            CreateMap<ParkingSlot, ParkingSlotAddDto>().ReverseMap();
            CreateMap<UserRegisterRequestDto, ApplicationUser>()
                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName))
                  .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                  .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.UserName.ToUpper()))
                  .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                  .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()))
                  .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true))
                  .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
        }
    }
}
