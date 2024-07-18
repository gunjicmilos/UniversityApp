using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UniversityController : ControllerBase
    {
        private readonly DataContext _context;
        public UniversityController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UniversityDto>>> GetUniversities([FromQuery] string? name = null, [FromQuery] string? location = null)
        {
            var universities = await _context.Universities
                .Select(u => new UniversityDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Location = u.Location,
                })
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(name))
            {
                universities = universities.Where(u => u.Name.Contains(name)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                universities = universities.Where(u => u.Location.Contains(location)).ToList();
            }

            return Ok(universities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UniversityDto>> GetUniversitiy(Guid id)
        {
            var universities = await _context.Universities
                .Select(u => new UniversityDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Location = u.Location,
                })
                .FirstOrDefaultAsync(u => u.Id == id);

            if (universities == null)
            {
                return NotFound($"University with id {id} not found");
            }

            return Ok(universities);
        }

        [HttpPost]
        public async Task<ActionResult<UniversityDto>> CreateUniversity(CreateUniversityDto createUniversityDto)
        {
            var university = new University()
            {
                Name = createUniversityDto.Name,
                Location = createUniversityDto.Location
            };
            
            _context.Universities.Add(university);
            await _context.SaveChangesAsync();

            var universityDto = new UniversityDto()
            {
                Id = university.Id,
                Name = university.Name,
                Location = university.Location
            };

            return Ok(universityDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUniversitiy(Guid id, CreateUniversityDto updateUniversityDto)
        {
            var university = await _context.Universities.FindAsync(id);

            if (university == null)
            {
                return NotFound($"University with id {id} not found");
            }

            university.Name = updateUniversityDto.Name;
            university.Location = updateUniversityDto.Location;

            _context.Entry(university).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUniversitiy(Guid id)
        {
            var university = await _context.Universities.FindAsync(id);

            if (university == null)
            {
                return NotFound($"University with id {id} not found");
            }

            _context.Universities.Remove(university);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}




