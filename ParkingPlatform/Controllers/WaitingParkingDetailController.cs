using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern;
using ParkingPlatform.Model;

namespace ParkingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaitingParkingDetailController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public WaitingParkingDetailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        [HttpGet]
        public async Task<IActionResult>GetAllWaitingParkingDetails()
        {
            var waitingparkingdetails=await _unitOfWork.WaitingParkingDetailsRepository.GetAllAsync();
            return Ok(waitingparkingdetails);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWaitingParkingDetailsById(int id)
        {
            var waitingparkingdetail=await _unitOfWork.WaitingParkingDetailsRepository.GetAsync(w=>w.Id==id);
            if (waitingparkingdetail == null)
            {
                return NotFound();
            }
            return Ok(waitingparkingdetail);
        }

        [HttpPost]
        public async Task<IActionResult> AddWaitingParkingDetail([FromBody]WaitingParkingDetail waitingParkingDetail)
        {
            if(waitingParkingDetail==null)
            {
                return BadRequest("waitingparkingdetails are invalid");
            }
            return CreatedAtAction(nameof(GetAllWaitingParkingDetails),new {id=waitingParkingDetail.Id },waitingParkingDetail);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWaitingParkingDetail(int id,[FromBody]WaitingParkingDetail waitingParkingDetail)
        {
            if (waitingParkingDetail == null)
            {
                return BadRequest("waitingparkingdetails are invalid");
            }
            var existing_waitingparkingdetail=await _unitOfWork.WaitingParkingDetailsRepository.GetAsync(w=>w.Id == id);
            if(existing_waitingparkingdetail == null)
            { 
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWaitingParkingDetail(int id)
        {
            var existing_waitingparkingdetail = await _unitOfWork.WaitingParkingDetailsRepository.GetAsync(w => w.Id == id);
            if(existing_waitingparkingdetail == null)
            { 
                return NotFound(); 
            }
            await _unitOfWork.WaitingParkingDetailsRepository.RemoveAsync(existing_waitingparkingdetail);
            _unitOfWork.Save();
            return NoContent();

        }
    }
}
