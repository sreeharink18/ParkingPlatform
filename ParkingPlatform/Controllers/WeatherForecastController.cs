using Microsoft.AspNetCore.Mvc;
using ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern;
using ParkingPlatform.Model;
using ParkingPlatform.Model.DTO.EmailDtosFolder;
using static System.Net.WebRequestMethods;

namespace ParkingPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private IUnitOfWork _unitOfWork;
        private IEmailServices _emailServices;
        public WeatherForecastController(ILogger<WeatherForecastController> logger,IUnitOfWork unitOfWork, IEmailServices emailServices)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _emailServices = emailServices;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet("GetAllUser")]
        public async Task<IEnumerable<ApplicationUser>> GetAllUser()
        {
            IEnumerable<ApplicationUser> applicationUsers = await _unitOfWork.ApplicationUserRepository.GetAllAsync();
            return applicationUsers;
        }
        [HttpPost("sentEmail")]
        public async Task<IActionResult> SendEmail(EmailSendDto requestDTO)
        {
            var message = new Message(requestDTO.Email, "OTP Confirmation", "830223");
            await _emailServices.SendEmailAsync(message, "SignupOTP");
            
            return Ok(message);
        }
    }
}
