using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers([FromQuery] string? name = null, [FromQuery] string? email = null, [FromQuery] Guid? facultyId = null)
        {
            var users = await _userService.GetUsersAsync(name, email, facultyId);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<UserDto>>> GetUser(Guid id)
        {
            var user = await _userService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound($"User with id : {id} does not exists");
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
            var user = await _userService.GetUserByEmailAsync(createUserDto.Email);
            if (user != null)
            {
                return NotFound($"User with email : {createUserDto.Email} already exists");
            }
            var result = await _userService.CreateUserAsync(createUserDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, CreateUserDto updateUserDto)
        {
            var user = await _userService.UpdateUserAsync(id, updateUserDto);

            if(user == null)
            {
                return NotFound($"User with id : {id} does not exists");
            }
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDto>> DeleteUser(Guid id)
        {
            var user = await _userService.DeleteUserAsync(id);

            if (user == null)
            {
                return NotFound($"User with id : {id} does not exists");
            }
            
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginRequest)
        {
            var user = await _userService.ValidateUserAsync(loginRequest.Username, loginRequest.Password);

            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = await _tokenService.GenerateTokenAsync(user);
            return Ok(new { Token = token });
        }
    }
}
