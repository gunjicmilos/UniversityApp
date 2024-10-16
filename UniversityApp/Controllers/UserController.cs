using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UniversityApp.Services.Interfaces;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ITokenService tokenService, ILogger<UserController> logger)
        {
            _userService = userService;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers([FromQuery] string? name = null, [FromQuery] string? email = null, [FromQuery] Guid? facultyId = null)
        {
            try
            {
                var users = await _userService.GetUsersAsync(name, email, facultyId);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching universities.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<UserDto>>> GetUser(Guid id)
        {
            try{
            var user = await _userService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound($"User with id : {id} does not exists");
            }

            return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching universities.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(createUserDto.Email);
                if (user != null)
                {
                    return NotFound($"User with email : {createUserDto.Email} already exists");
                }

                var result = await _userService.CreateUserAsync(createUserDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching universities.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, CreateUserDto updateUserDto)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(id, updateUserDto);

                if (user == null)
                {
                    return NotFound($"User with id : {id} does not exists");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching universities.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDto>> DeleteUser(Guid id)
        {
            try
            {
                var user = await _userService.DeleteUserAsync(id);

                if (user == null)
                {
                    return NotFound($"User with id : {id} does not exists");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching universities.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginRequest)
        {
            try {
                var user = await _userService.ValidateUserAsync(loginRequest.Username, loginRequest.Password);
                if (user == null)
                {
                    return Unauthorized("Invalid credentials.");
                }

                var token = await _tokenService.GenerateTokenAsync(user);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching universities.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [HttpPost("login2")]
        public string Login2([FromBody] LoginDto loginRequest)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("your_secret_key");
    
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, loginRequest.Username),
                    new Claim(ClaimTypes.Role, "Admin") // Dodaj rolu u token
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);        } 
    }
}
