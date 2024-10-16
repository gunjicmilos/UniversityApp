using Microsoft.AspNetCore.Mvc;
using UniversityApp.Controllers;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UniversityController : ControllerBase
    {
        private readonly IUniversityService _universityService;
        private readonly ILogger<ChatBotController> _logger;

        public UniversityController(IUniversityService universityService, ILogger<ChatBotController> logger)
        {
            _universityService = universityService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<UniversityDto>>> GetUniversities([FromQuery] string? name = null, [FromQuery] string? location = null)
        {
            try
            {
                var universities = await _universityService.GetUniversities(name, location);
                return Ok(universities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching universities.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UniversityDto>> GetUniversitiy(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var universities = await _universityService.GetUniversitiy(id);

                if (universities == null)
                {
                    return NotFound($"University with id {id} not found");
                }

                return Ok(universities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching universities.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }   
        }

        //[Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<ActionResult<UniversityDto>> CreateUniversity(CreateUniversityDto createUniversityDto)
        {
            try
            {
                if (!await _universityService.UniversityExists(createUniversityDto.Name))
                {
                    var university = await _universityService.CreateUniversity(createUniversityDto);
                    return Ok(university);
                }
                else
                {
                    return BadRequest($"University {createUniversityDto.Name} already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching universities.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        //[Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUniversitiy(Guid id, CreateUniversityDto updateUniversityDto)
        {
            var university = await _universityService.UpdateUniversitiy(id, updateUniversityDto);

            if (university == null)
            {
                return NotFound($"University with id {id} not found");
            }

            return NoContent();
        }

        //[Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUniversitiy(Guid id)
        {
            try
            {
                var university = await _universityService.DeleteUniversitiy(id);

                if (university == null)
                {
                    return NotFound($"University with id {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching universities.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }
    }
}




