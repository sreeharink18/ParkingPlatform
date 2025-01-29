using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern;
using ParkingPlatform.Model.DTO.VehicleTypeDtosFolder;
using ParkingPlatform.Model;
using ParkingPlatform.Model.DTO.GateDtosFolder;
using ParkingPlatform.Model.DTO.ParkingSlotDtosFolder;
using ParkingPlatform.Model.DTO;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using ParkingPlatform.Utility;
namespace ParkingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GateController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GateController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllGate()
        {
            var gates = await _unitOfWork.GateRepository.GetAllAsync();
            return Ok(ApiResponseHelper.SuccessResponse(gates));
        }

        


        [HttpGet("{gate_id}")]
        public async Task<IActionResult> GetGateById(int gate_id)
        {
            var gate = await _unitOfWork.GateRepository.GetAsync(v => v.Id == gate_id);
            if (gate == null)
            {
                return NotFound(ApiResponseHelper.ErrorResponse("Not found",HttpStatusCode.NotFound));
            }

            return Ok(ApiResponseHelper.SuccessResponse(gate));
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AddGate([FromBody] GateAddDto gate)
        {
            if (gate == null)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse("Gate inputs are invalid"));
            }
            var vehicle_type=await _unitOfWork.VehicleRepository.GetAsync(v=>v.Id==gate.VehicleTypeId);
            if (vehicle_type == null)
            {
                return BadRequest("Invalid VehicleType Id");
            }
            TimeSpan penaltyTime;
            if (!TimeSpan.TryParse(gate.PenaltyTime, out penaltyTime))
            {
                return BadRequest(ApiResponseHelper.ErrorResponse("Invalid PenaltyTime format. Please use 'hh:mm:ss'."));
            }
            var new_gate = _mapper.Map<Gate>(gate);
            new_gate.PenaltyTime = penaltyTime;
            await _unitOfWork.GateRepository.AddAsync(new_gate);
            _unitOfWork.Save();
            for (int i=1;i<=gate.SlotSize;i++)
            {
                var parkingslot = new ParkingSlotAddDto
                {
                    GateId = new_gate.Id,
                    ParkingSlotNumber = i,
                    Status = "Available"
                };
                var park_slot = _mapper.Map<ParkingSlot>(parkingslot);
                await _unitOfWork.ParkingSlotRepository.AddAsync(park_slot);
                _unitOfWork.Save();
            }
            return Ok(ApiResponseHelper.SuccessResponse("Created"));
        }

        [HttpPut("{gate_id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateGate(int gate_id, [FromBody] GateAddDto gate)
        {
            if (gate == null)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse("Gate data is invalid"));
            }
            var existing_gate = await _unitOfWork.GateRepository.GetAsync(v => v.Id == gate_id);
            if (existing_gate == null)
            {
                return NotFound();
            }
            TimeSpan penaltyTime;
            if (!TimeSpan.TryParse(gate.PenaltyTime, out penaltyTime))
            {
                return BadRequest(ApiResponseHelper.ErrorResponse("Invalid PenaltyTime format. Please use 'hh:mm:ss'."));
            }
            if (existing_gate.SlotSize>gate.SlotSize)
            {
                for(int i=gate.SlotSize+1; i<=existing_gate.SlotSize; i++)
                {
                    var remove_parkingslot = await _unitOfWork.ParkingSlotRepository.GetAsync(p=>p.ParkingSlotNumber == i);
                    await _unitOfWork.ParkingSlotRepository.RemoveAsync(remove_parkingslot);
                     _unitOfWork.Save();
                }
            }
            if(existing_gate.SlotSize < gate.SlotSize)
            {
                for (int i = 1; i <=gate.SlotSize- existing_gate.SlotSize; i++)
                {
                    var parkingslot = new ParkingSlotAddDto
                    {
                        GateId = gate_id,
                        ParkingSlotNumber = existing_gate.SlotSize + i,
                        Status = "Available"
                    };
                    var park_slot = _mapper.Map<ParkingSlot>(parkingslot);
                    await _unitOfWork.ParkingSlotRepository.AddAsync(park_slot);
                    _unitOfWork.Save();
                }
            }
            existing_gate.PenaltyTime = penaltyTime;
            existing_gate.SlotSize = gate.SlotSize;
            existing_gate.VehicleTypeId=gate.VehicleTypeId;
            existing_gate.GateName = gate.GateName;
            await _unitOfWork.GateRepository.UpdateAsync(existing_gate);
            _unitOfWork.Save();
            
            return Ok(ApiResponseHelper.SuccessResponse("Updated",HttpStatusCode.NoContent));
        }

        [HttpDelete("{gate_id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteGate(int gate_id)
        {
            var gate = await _unitOfWork.GateRepository.GetAsync(v => v.Id == gate_id);
            if (gate == null)
            {
                return NotFound();
            }
           
            var parkingSlots = await _unitOfWork.ParkingSlotRepository.GetAllAsync(p => p.GateId == gate_id);

            if (parkingSlots != null && parkingSlots.Any())
            {
                foreach (var parkingSlot in parkingSlots)
                {
                    await _unitOfWork.ParkingSlotRepository.RemoveAsync(parkingSlot);
                    _unitOfWork.Save();
                }
            }
            await _unitOfWork.GateRepository.RemoveAsync(gate);
            _unitOfWork.Save();

            return Ok(ApiResponseHelper.SuccessResponse("Deleted", HttpStatusCode.NoContent));
        }
    }
}
