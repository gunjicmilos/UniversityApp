using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Services.Interfaces;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class FacultyController : ControllerBase
    {
        private readonly IFacultyService _facultyService;
        private readonly ILogger<FacultyController> _logger;

        public FacultyController(IFacultyService facultyService, ILogger<FacultyController> logger)
        {
            _facultyService = facultyService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacultyDto>>> GetFaculties([FromQuery] string? name = null, [FromQuery] string? location = null, [FromQuery] Guid? UniversityId = null)
        {
            try
            {
                var faculties = await _facultyService.GetFaculties(name, location, UniversityId);
                return Ok(faculties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching faculties.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }   

        [HttpGet("{id}")]
        public async Task<ActionResult<FacultyDto>> GetFaculty(Guid id)
        {
            try
            {
                var faculties = await _facultyService.GetFaculty(id);

                if (faculties == null)
                {
                    return NotFound($"Faculty with id {id} not found");
                }

                return Ok(faculties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching faculties.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<ActionResult<FacultyDto>> CreateFaculty(CreateFacultyDto createFacultyDto)
        {
            try
            {
                if (!await _facultyService.FacultyExists(createFacultyDto.Name))
                {
                    var faculty = await _facultyService.CreateFaculty(createFacultyDto);
                    return Ok(faculty);
                }
                else
                {
                    return BadRequest($"Faculty {createFacultyDto.Name} already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating faculties.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFaculty(Guid id, CreateFacultyDto updateFacultyDto)
        {
            try
            {
                var faculty = await _facultyService.UpdateFaculty(id, updateFacultyDto);

                if (faculty == null)
                {
                    return NotFound($"Faculty with id {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating faculties.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFaculty(Guid id)
        {
            try
            {
                var faculty = await _facultyService.DeleteFaculty(id);

                if (faculty == null)
                {
                    return NotFound($"Faculty with id {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting faculties.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("/addUser")]
        public async Task<ActionResult> AddUserToFaculty(AssignUserDto addUserDto)
        {
            try
            {
                if (await _facultyService.IsUserAddedToFaculty(addUserDto))
                {
                    return BadRequest("User is already added to faculty");
                }

                var faculty = await _facultyService.AddUserToFaculty(addUserDto);
                if (faculty == null)
                {
                    return NotFound("Faculty or User does not exists");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding users to faculties.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }
    }
}
