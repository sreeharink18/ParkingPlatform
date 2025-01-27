using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern;
using ParkingPlatform.Model.DTO.VehicleTypeDtosFolder;
using ParkingPlatform.Model;
using ParkingPlatform.Model.DTO.GateDtosFolder;

namespace ParkingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GateController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GateController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllGate()
        {
            var gates = await _unitOfWork.GateRepository.GetAllAsync();
            return Ok(gates);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetGateById(int id)
        {
            var gate = await _unitOfWork.GateRepository.GetAsync(v => v.Id == id);
            if (gate == null)
            {
                return NotFound();
            }

            return Ok(gate);
        }

        [HttpPost]
        public async Task<IActionResult> AddGate([FromBody] GateAddDto gate)
        {
            if (gate == null)
            {
                return BadRequest("Vehicle is null");
            }
            TimeSpan penaltyTime;
            if (!TimeSpan.TryParse(gate.PenaltyTime, out penaltyTime))
            {
                return BadRequest("Invalid PenaltyTime format. Please use 'hh:mm:ss'.");
            }
            Console.WriteLine($"Penalty Time (Total Seconds): {penaltyTime.TotalSeconds}");
            Console.WriteLine($"Penalty Time (Formatted): {penaltyTime.ToString(@"hh\:mm\:ss")}");

            var new_gate = _mapper.Map<Gate>(gate);
            new_gate.PenaltyTime = penaltyTime; 
            await _unitOfWork.GateRepository.AddAsync(new_gate);
            _unitOfWork.Save();

            return CreatedAtAction(nameof(GetAllGate), new { id = new_gate.Id }, gate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGate(int id, [FromBody] GateAddDto gate)
        {
            if (gate == null)
            {
                return BadRequest("Vehicle data is invalid");
            }

            var existing_gate = await _unitOfWork.GateRepository.GetAsync(v => v.Id == id);
            if (existing_gate == null)
            {
                return NotFound();
            }
            TimeSpan penaltyTime;
            if (!TimeSpan.TryParse(gate.PenaltyTime, out penaltyTime))
            {
                return BadRequest("Invalid PenaltyTime format. Please use 'hh:mm:ss'.");
            }
            existing_gate.PenaltyTime = penaltyTime;
            existing_gate.SlotSize = gate.SlotSize;
            existing_gate.VehicleTypeId=gate.VehicleTypeId;
            existing_gate.GateName = gate.GateName;
            await _unitOfWork.GateRepository.UpdateAsync(existing_gate);
            _unitOfWork.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGate(int id)
        {
            var gate = await _unitOfWork.GateRepository.GetAsync(v => v.Id == id);
            if (gate == null)
            {
                return NotFound();
            }

            await _unitOfWork.GateRepository.RemoveAsync(gate);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
