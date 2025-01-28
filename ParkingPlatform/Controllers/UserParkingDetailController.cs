using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern;
using ParkingPlatform.Model;
using ParkingPlatform.Model.DTO;
using ParkingPlatform.Model.DTO.UserParkingDetailDtosFolder;
using ParkingPlatform.Utility;

namespace ParkingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserParkingDetailController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private IEmailServices _emailServices;
        private IMapper _mapper;

        public UserParkingDetailController(IUnitOfWork unitOfWork,IEmailServices emailServices,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _emailServices = emailServices;
            _mapper = mapper;
        }
        [HttpPost("enterUserParking")]
        public async Task<IActionResult> EnterUserParkingDetail([FromBody] CreateUserParkingDetailRequestDto model)
        {
            try
            {
                if (ModelState.IsValid) {
                    ParkingSlot parkingSlot = await AvailableParkingSlot(model.VehicleType);
                    if (parkingSlot != null) { 
                        UserParkingDetail userParkingDetailDto =  _mapper.Map<UserParkingDetail>(model);
                        userParkingDetailDto.ArrivalTime = DateTime.Now;
                        userParkingDetailDto.ParkingSlotId = parkingSlot.Id;
                        _unitOfWork.UserParkingDetailsRepository.AddAsync(userParkingDetailDto);

                        // Status update
                        parkingSlot.Status = StaticData.ParkingStatus_Engaged;
                        _unitOfWork.ParkingSlotRepository.UpdateAsync(parkingSlot);
                        _unitOfWork.Save();
                        return Ok(ApiResponseHelper.SuccessResponse(parkingSlot));
                    }
                    else
                    {
                       WaitingParkingDetail waitingParkingDetailDto =  _mapper.Map<WaitingParkingDetail>(model);
                        waitingParkingDetailDto.ArrivalWaitingTime = DateTime.Now;
                        _unitOfWork.WaitingParkingDetailsRepository.AddAsync(waitingParkingDetailDto).Wait();
                        _unitOfWork.Save();
                        return Ok(ApiResponseHelper.SuccessResponse("Your add to waiting list... Plz wait sometime"));
                    }
                }
                return BadRequest(ApiResponseHelper.ErrorResponse("something went wrong"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse(ex.Message));
            }
        }
        
        private async Task<ParkingSlot> AvailableParkingSlot(string vechileType)
        {
            VehicleType vehicleTypeFromDb = await _unitOfWork.VehicleRepository.GetAsync(u => u.VehicleTypeName == vechileType);
            IEnumerable<Gate> getAllGateByVechileType = await _unitOfWork.GateRepository.GetAllAsync(u=>u.VehicleTypeId == vehicleTypeFromDb.Id);

            var allParkingSlot = await _unitOfWork.ParkingSlotRepository.GetAllAsync(u=>u.Gate.VehicleTypeId == vehicleTypeFromDb.Id && u.Status == StaticData.ParkingStatus_Available);
            ParkingSlot parkingSlotFirst = allParkingSlot.FirstOrDefault();
            return parkingSlotFirst;
        }
    }
}
