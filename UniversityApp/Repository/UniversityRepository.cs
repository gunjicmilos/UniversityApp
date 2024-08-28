using Microsoft.EntityFrameworkCore;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository;

public class UniversityRepository : IUniversityRepository
{
    private readonly DataContext _context;

    public UniversityRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<UniversityDto>> GetAllUniversityAsync()
    {
        return await _context.Universities
            .Select(u => new UniversityDto
            {
                Id = u.Id,
                Name = u.Name,
                Location = u.Location,
            })
            .ToListAsync();
    }

    public async Task<UniversityDto> GetUniversityAsync(Guid id)
    {
        return await _context.Universities
            .Select(u => new UniversityDto
            {
                Id = u.Id,
                Name = u.Name,
                Location = u.Location,
            })
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task CreateUniversityAsync(University university)
    {
        _context.Universities.Add(university);
        await _context.SaveChangesAsync();
    }

    public async Task<University> UniversityExistsAsync(string name)
    {
        return await _context.Universities.FirstOrDefaultAsync(u => u.Name == name);
    }

    public async Task<University> FindUniversityAsync(Guid id)
    {
        return await _context.Universities.FindAsync(id);;
    }

    public async Task UpdateUniversityAsync(Guid id, University updateUniversityDto)
    {
        _context.Universities.Update(updateUniversityDto);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUniversityAsync(University university)
    {
        _context.Universities.Remove(university);
        await _context.SaveChangesAsync();
    }
}