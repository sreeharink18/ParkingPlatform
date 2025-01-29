using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern;
using ParkingPlatform.Model;
using ParkingPlatform.Model.DTO;
using System.Net;
namespace ParkingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            return Ok(ApiResponseHelper.SuccessResponse( waitingparkingdetails));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWaitingParkingDetailsById(int id)
        {
            var waitingparkingdetail=await _unitOfWork.WaitingParkingDetailsRepository.GetAsync(w=>w.Id==id);
            if (waitingparkingdetail == null)
            {
                return NotFound(ApiResponseHelper.ErrorResponse("Not found",HttpStatusCode.NotFound));
            }
            return Ok(ApiResponseHelper.SuccessResponse(waitingparkingdetail));
        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteWaitingParkingDetail(int id)
        {
            var existing_waitingparkingdetail = await _unitOfWork.WaitingParkingDetailsRepository.GetAsync(w => w.Id == id);
            if(existing_waitingparkingdetail == null)
            { 
                return NotFound(ApiResponseHelper.ErrorResponse("Not found",HttpStatusCode.NotFound)); 
            }
            await _unitOfWork.WaitingParkingDetailsRepository.RemoveAsync(existing_waitingparkingdetail);
            _unitOfWork.Save();
            return Ok(ApiResponseHelper.SuccessResponse("Deleted",HttpStatusCode.NoContent));

        }
    }
}
