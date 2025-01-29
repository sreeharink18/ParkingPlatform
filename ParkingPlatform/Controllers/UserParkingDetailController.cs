using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern;
using ParkingPlatform.Model;
using ParkingPlatform.Model.DTO;
using ParkingPlatform.Model.DTO.EmailDtosFolder;
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
        [HttpPut("ExitUserParking{parkingSlotId}")]
        public async Task<IActionResult> ExitUserParkingSlot(int parkingSlotId)
        {
            try
            {
                ParkingSlot parkingSlotFromDb = await _unitOfWork.ParkingSlotRepository.GetAsync(u => u.Id == parkingSlotId,"Gate");
                VehicleType vehicleTypeFromDb = await _unitOfWork.VehicleRepository.GetAsync(u=>u.Id == parkingSlotFromDb.Gate.VehicleTypeId);
                UserParkingDetail userParkingDetailFromDb = await _unitOfWork.UserParkingDetailsRepository.GetAsync(u=>u.ParkingSlotId == parkingSlotId && u.DepartureTime ==null,"ApplicationUser");
                IEnumerable<WaitingParkingDetail> waitingParkingDetailList = await _unitOfWork.WaitingParkingDetailsRepository.GetAllAsync(u=>u.VehicleType == vehicleTypeFromDb.VehicleTypeName);
                if (waitingParkingDetailList.Count() > 0) { 
                    WaitingParkingDetail firstWaitingParkingDetail = waitingParkingDetailList.FirstOrDefault();
                    UserParkingDetail newUserParkingDetail = new()
                    {
                        UserId = firstWaitingParkingDetail.UserId,
                        ArrivalTime = DateTime.Now,
                        ParkingSlotId = parkingSlotFromDb.Id,
                        VehicleNumber = firstWaitingParkingDetail.VehicleNumber,   
                    };
                    _unitOfWork.UserParkingDetailsRepository.AddAsync(newUserParkingDetail);
                    _unitOfWork.WaitingParkingDetailsRepository.RemoveAsync(firstWaitingParkingDetail);
                    //Setup Email for next user
                }
                else
                {
                    parkingSlotFromDb.Status = StaticData.ParkingStatus_Available;
                    _unitOfWork.ParkingSlotRepository.UpdateAsync(parkingSlotFromDb);
                }
                userParkingDetailFromDb.DepartureTime = DateTime.Now;
                _unitOfWork.UserParkingDetailsRepository.Update(userParkingDetailFromDb);
                _unitOfWork.Save();
                UserParkingDetailResponseDto userParkingDetailResponse = await GetAllParkingDetailResponse(userParkingDetailFromDb,parkingSlotFromDb,vehicleTypeFromDb);
                var textContent = $"Dear {userParkingDetailResponse.UserName}, your vehicle {userParkingDetailResponse.VehicleNumber} has exited the parking slot. You parked at Gate {userParkingDetailResponse.GateName}, Slot {userParkingDetailResponse.ParkingSlotNumber}, from {userParkingDetailResponse.Arrival} to {userParkingDetailResponse.Depature}, totaling {userParkingDetailResponse.TotalTimeSpend}. Your parking charge is {userParkingDetailResponse.HourlyCharge} per hour, with an additional penalty charge of {userParkingDetailResponse.PenaltyCharge} (if applicable). Your total amount due is {userParkingDetailResponse.TotalAmount}. Thank you for using our parking service! ";
                var message = new Message(userParkingDetailFromDb.ApplicationUser.Email, "Exit Parking Slot", textContent);
                await _emailServices.SendEmailAsync(message, "ExitParkingSlot");
                return Ok(ApiResponseHelper.SuccessResponse(userParkingDetailResponse));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse(ex.Message));
            }
        }
        private async Task<UserParkingDetailResponseDto> GetAllParkingDetailResponse(UserParkingDetail userParkingDetail, ParkingSlot parkingSlot,VehicleType vehicleType)
        {
            DateTime depatureTime = DateTime.Now;
            
            TimeSpan differnce = depatureTime - userParkingDetail.ArrivalTime;
            Console.WriteLine(differnce.Seconds + " " + differnce.Days + " " + differnce.TotalDays);
            int totalHours = (int)(Math.Ceiling(differnce.TotalHours));
            decimal totalAmount = totalHours * vehicleType.HourlyCharge;
            if (differnce > parkingSlot.Gate.PenaltyTime) {
                totalAmount += vehicleType.PenaltyCharge;
            }
            return new UserParkingDetailResponseDto { 
                GateName = parkingSlot.Gate.GateName,
                PenaltyCharge = vehicleType.PenaltyCharge,
                Arrival = userParkingDetail.ArrivalTime,
                Depature = depatureTime,
                HourlyCharge = vehicleType.HourlyCharge,
                TotalAmount = totalAmount,
                ParkingSlotNumber = parkingSlot.ParkingSlotNumber,
                TotalTimeSpend = differnce,
                Email = userParkingDetail.ApplicationUser.Email,
                UserName = userParkingDetail.ApplicationUser.Name,
                PhoneNumber = userParkingDetail.ApplicationUser.PhoneNumber,
                VehicleNumber = userParkingDetail.VehicleNumber,
            };

        }
    }
}
