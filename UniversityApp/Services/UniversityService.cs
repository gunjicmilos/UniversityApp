using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Services;

public class UniversityService : IUniversityService
{
    private readonly DataContext _context;
        public UniversityService(DataContext context)
        {
            _context = context;
        }

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

            return universities;
        }

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

            return universities;
        }

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

            return universityDto;
        }

        public async Task<University> UpdateUniversitiy(Guid id, CreateUniversityDto updateUniversityDto)
        {
            var university = await _context.Universities.FindAsync(id);


            university.Name = updateUniversityDto.Name;
            university.Location = updateUniversityDto.Location;

            _context.Entry(university).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return university;
        }

        public async Task<University> DeleteUniversitiy(Guid id)
        {
            var university = await _context.Universities.FindAsync(id);
            
            _context.Universities.Remove(university);
            await _context.SaveChangesAsync();

            return university;
        }
}