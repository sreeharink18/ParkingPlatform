using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern;
using ParkingPlatform.Model;
using ParkingPlatform.Model.DTO;
using ParkingPlatform.Model.DTO.ApplicationUserDtosFolder;
using ParkingPlatform.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace ParkingPlatform.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private string secretKey;

        public AuthController(IConfiguration configuration, IUnitOfWork unitOfWork,
             SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            secretKey = configuration.GetValue<string>("ApiSettings:secret");
            _mapper = mapper;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto model)
        {

            ApplicationUser userFromDb = await _unitOfWork.ApplicationUserRepository.GetAsync(u => u.UserName == model.UserName);
            if (userFromDb == null)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse("Email is not Exist Plz check once more"));
            }
            if (userFromDb.LockoutEnd.HasValue && userFromDb.LockoutEnd > DateTimeOffset.UtcNow)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse("User is locked out. Please try again later.", HttpStatusCode.Forbidden));
            }

            bool isValid = await _userManager.CheckPasswordAsync(userFromDb, model.Password);
            if (isValid == false)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse("UserName or Password is incorrect"));
            }

            var roles = await _userManager.GetRolesAsync(userFromDb);
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(secretKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("fullName" ,userFromDb.Name),
                    new Claim("id" ,userFromDb.Id.ToString()),
                    new Claim(ClaimTypes.Email,userFromDb.UserName),
                    new Claim(ClaimTypes.Role,roles.FirstOrDefault()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            UserLoginResponseDto response = new()
            {
                Email = userFromDb.UserName,
                Token = tokenHandler.WriteToken(token),
            };
            if (response.Email == null || string.IsNullOrEmpty(response.Token))
            {
                return BadRequest(ApiResponseHelper.ErrorResponse("UserName or Password is incorrect"));
            }
            return Ok(ApiResponseHelper.SuccessResponse(response));
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequestDto model)
        {
            ApplicationUser UserFromDb = await _unitOfWork.ApplicationUserRepository.GetAsync(u => u.UserName.ToLower() == model.UserName.ToLower());

            if (UserFromDb != null)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse("User is already exist"));
            }
            var newUserDto = _mapper.Map<ApplicationUser>(model);
            
            try
            {
                var result = await _userManager.CreateAsync(newUserDto, model.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(StaticData.Role_Admin).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticData.Role_Admin));
                        await _roleManager.CreateAsync(new IdentityRole(StaticData.Role_Customer));
                    }
                    
                        await _userManager.AddToRoleAsync(newUserDto, StaticData.Role_Customer);
                    
                    return Ok(ApiResponseHelper.SuccessResponse("User is Register Successfully! Plz countinue..."));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse(ex.Message));
            }
            return BadRequest(ApiResponseHelper.ErrorResponse("Internal Server Issue...", HttpStatusCode.InternalServerError));
        }
    }
}
