using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;

        public UserController(IUserService userService, ITokenService tokenService, ILogger logger)
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
    }
}
