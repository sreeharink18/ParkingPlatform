using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern;
using ParkingPlatform.Model;
using ParkingPlatform.Model.DTO;
using ParkingPlatform.Model.DTO.VehicleTypeDtosFolder;
using System.Net;
namespace ParkingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public VehicleController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;

          _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = await _unitOfWork.VehicleRepository.GetAllAsync();
            return Ok(ApiResponseHelper.SuccessResponse(vehicles));
        }

       

        [HttpGet("{vehicle_id}")]
        public async Task<IActionResult> GetVehicleById(int vehicle_id)
        {
            var vehicle = await _unitOfWork.VehicleRepository.GetAsync(v=>v.Id== vehicle_id);
            if (vehicle == null)
            {
                return NotFound(ApiResponseHelper.ErrorResponse("not found",HttpStatusCode.NotFound));
            }

            return Ok(ApiResponseHelper.SuccessResponse(vehicle));  
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleTypeAddDto  vehicle)
        {
            if (vehicle == null)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse( "Vehicle is null",HttpStatusCode.BadRequest));
            }
            var new_vehicle=_mapper.Map<VehicleType>(vehicle);
            await _unitOfWork.VehicleRepository.AddAsync(new_vehicle);  
            _unitOfWork.Save();  

            return Ok(ApiResponseHelper.SuccessResponse("created",HttpStatusCode.NoContent));
        }

        [HttpPut("{vehicle_id}")]
        public async Task<IActionResult> UpdateVehicle(int vehicle_id, [FromBody] VehicleTypeAddDto vehicle)
        {
            if (vehicle == null )
            {
                return BadRequest(ApiResponseHelper.ErrorResponse("Vehicle data is invalid",HttpStatusCode.BadRequest));
            }

            var existingVehicle = await _unitOfWork.VehicleRepository.GetAsync(v=>v.Id== vehicle_id);
            if (existingVehicle == null)
            {
                return NotFound(ApiResponseHelper.ErrorResponse("Not found",HttpStatusCode.NotFound));
            }
            existingVehicle.PenaltyCharge = vehicle.PenaltyCharge;
            existingVehicle.HourlyCharge = vehicle.HourlyCharge;
            existingVehicle.VehicleTypeName = vehicle.VehicleTypeName;
            //var updated_vehicle = _mapper.Map<VehicleType>(vehicle);
            await _unitOfWork.VehicleRepository.UpdateAsync(existingVehicle);  
            _unitOfWork.Save();  

            return Ok(ApiResponseHelper.SuccessResponse("Updated",HttpStatusCode.NoContent));  
        }

        [HttpDelete("{vehicle_id}")]
        public async Task<IActionResult> DeleteVehicle(int vehicle_id)
        {
            var vehicle = await _unitOfWork.VehicleRepository.GetAsync(v=>v.Id== vehicle_id);
            if (vehicle == null)
            {
                return NotFound(ApiResponseHelper.ErrorResponse("Not found",HttpStatusCode.NotFound));
            }

            await _unitOfWork.VehicleRepository.RemoveAsync(vehicle);  
            _unitOfWork.Save();  

            return Ok(ApiResponseHelper.SuccessResponse("Deleted",HttpStatusCode.NoContent));  
        }
    }
}
