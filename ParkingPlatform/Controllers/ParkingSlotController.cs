using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern;
using ParkingPlatform.Model.DTO.VehicleTypeDtosFolder;
using ParkingPlatform.Model;
using ParkingPlatform.Model.DTO.ParkingSlotDtosFolder;
using ParkingPlatform.Model.DTO.GateDtosFolder;

namespace ParkingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingSlotController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ParkingSlotController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllParkingSlot()
        {
            var parkingslot = await _unitOfWork.ParkingSlotRepository.GetAllAsync();
            return Ok(parkingslot);
        }



        [HttpGet("get_all_parkingslot_by_gate/{gate_id}")]
        public async Task<IActionResult> GetAllParkingSlotByGate_Id(int gate_id)
        {
            var parkingSlot = await _unitOfWork.ParkingSlotRepository.GetAllAsync(v => v.GateId == gate_id );
            if (parkingSlot == null)
            {
                return NotFound();
            }

            return Ok(parkingSlot);
        }


        [HttpGet("get_all_parkingslot_by_vehicletype/{vehicle_id}")]
        public async Task<IActionResult> GetAllParkingSlotByVehicleType(int vehicle_id)
        {
            var parkingSlots = await _unitOfWork.ParkingSlotRepository.GetAllAsync( ps => ps.Gate.VehicleTypeId == vehicle_id);
            return Ok(parkingSlots);
        }

        [HttpGet("{parkingslot_id}")]
        public async Task<IActionResult> GetParkingSlotById(int parkingslot_id)
        {
            var parkingSlot = await _unitOfWork.ParkingSlotRepository.GetAsync(v => v.Id == parkingslot_id);
            if (parkingSlot == null)
            {
                return NotFound();
            }

            return Ok(parkingSlot);
        }

        [HttpPost]
        public async Task<IActionResult> AddParkingSlot([FromBody] ParkingSlotAddDto parkingslot)
        {
            if (parkingslot == null)
            {
                return BadRequest("Vehicle is null");
            }
            var new_parkingslot = _mapper.Map<ParkingSlot>(parkingslot);
            var gate=await _unitOfWork.GateRepository.GetAsync(g=>g.Id==parkingslot.GateId);
            gate.SlotSize = gate.SlotSize + 1;
            await _unitOfWork.GateRepository.UpdateAsync(gate);
            await _unitOfWork.ParkingSlotRepository.AddAsync(new_parkingslot);
            _unitOfWork.Save();

            return CreatedAtAction(nameof(GetAllParkingSlot), new { id = new_parkingslot.Id }, parkingslot);
        }

        [HttpPut("{parkingslot_id}")]
        public async Task<IActionResult> UpdateParkingSlot(int parkingslot_id, [FromBody] ParkingSlotAddDto parkingSlot)
        {
            if (parkingSlot == null)
            {
                return BadRequest("Vehicle data is invalid");
            }

            var existing_parkingslot = await _unitOfWork.ParkingSlotRepository.GetAsync(v => v.Id == parkingslot_id);
            if (existing_parkingslot == null)
            {
                return NotFound();
            }
            existing_parkingslot.ParkingSlotNumber = parkingSlot.ParkingSlotNumber;
            existing_parkingslot.Status = parkingSlot.Status;
            existing_parkingslot.GateId = parkingSlot.GateId;
            await _unitOfWork.ParkingSlotRepository.UpdateAsync(existing_parkingslot);
            _unitOfWork.Save();

            return NoContent();
        }

        [HttpDelete("{parkingslot_id}")]
        public async Task<IActionResult> DeleteParkingSlot(int parkingslot_id)
        {
            var parkingSlot = await _unitOfWork.ParkingSlotRepository.GetAsync(v => v.Id == parkingslot_id);
            if (parkingSlot == null)
            {
                return NotFound();
            }
            var gate=await _unitOfWork.GateRepository.GetAsync(g=>g.Id == parkingSlot.GateId);
            gate.SlotSize =gate.SlotSize- 1;
            await _unitOfWork.GateRepository.UpdateAsync(gate);
            await _unitOfWork.ParkingSlotRepository.RemoveAsync(parkingSlot);
            _unitOfWork.Save();

            return NoContent();
        }

    }
}
