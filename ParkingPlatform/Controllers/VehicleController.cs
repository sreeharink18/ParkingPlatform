using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern;
using ParkingPlatform.Model;
using ParkingPlatform.Model.DTO.VehicleTypeDtosFolder;

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
            return Ok(vehicles);
        }



        [HttpGet("{vehicle_id}")]
        public async Task<IActionResult> GetVehicleById(int vehicle_id)
        {
            var vehicle = await _unitOfWork.VehicleRepository.GetAsync(v=>v.Id== vehicle_id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);  
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleTypeAddDto  vehicle)
        {
            if (vehicle == null)
            {
                return BadRequest("Vehicle is null");
            }
            var new_vehicle=_mapper.Map<VehicleType>(vehicle);
            await _unitOfWork.VehicleRepository.AddAsync(new_vehicle);  
            _unitOfWork.Save();  

            return CreatedAtAction(nameof(GetAllVehicles), new { id = new_vehicle.Id }, vehicle);
        }

        [HttpPut("{vehicle_id}")]
        public async Task<IActionResult> UpdateVehicle(int vehicle_id, [FromBody] VehicleTypeAddDto vehicle)
        {
            if (vehicle == null )
            {
                return BadRequest("Vehicle data is invalid");
            }

            var existingVehicle = await _unitOfWork.VehicleRepository.GetAsync(v=>v.Id== vehicle_id);
            if (existingVehicle == null)
            {
                return NotFound();
            }
            existingVehicle.PenaltyCharge = vehicle.PenaltyCharge;
            existingVehicle.HourlyCharge = vehicle.HourlyCharge;
            existingVehicle.VehicleTypeName = vehicle.VehicleTypeName;
            //var updated_vehicle = _mapper.Map<VehicleType>(vehicle);
            await _unitOfWork.VehicleRepository.UpdateAsync(existingVehicle);  
            _unitOfWork.Save();  

            return NoContent();  
        }

        [HttpDelete("{vehicle_id}")]
        public async Task<IActionResult> DeleteVehicle(int vehicle_id)
        {
            var vehicle = await _unitOfWork.VehicleRepository.GetAsync(v=>v.Id== vehicle_id);
            if (vehicle == null)
            {
                return NotFound();
            }

            await _unitOfWork.VehicleRepository.RemoveAsync(vehicle);  
            _unitOfWork.Save();  

            return NoContent();  
        }
    }
}
