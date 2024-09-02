using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UniversityController : ControllerBase
    {
        private readonly IUniversityService _universityService;
        public UniversityController(IUniversityService universityService)
        {
            _universityService = universityService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UniversityDto>>> GetUniversities([FromQuery] string? name = null, [FromQuery] string? location = null)
        {
            var universities = await _universityService.GetUniversities(name, location);
            return Ok(universities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UniversityDto>> GetUniversitiy(Guid id)
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

        [HttpPost]
        public async Task<ActionResult<UniversityDto>> CreateUniversity(CreateUniversityDto createUniversityDto)
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUniversitiy(Guid id)
        {
            var university = await _universityService.DeleteUniversitiy(id);

            if (university == null)
            {
                return NotFound($"University with id {id} not found");
            }

            return NoContent();
        }
    }
}




