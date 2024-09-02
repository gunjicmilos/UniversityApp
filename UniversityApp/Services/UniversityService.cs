using UniversityApp.Repository.IRepository;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Services;

public class UniversityService : IUniversityService
{
    private readonly IUniversityRepository _universityRepository;
    public UniversityService(DataContext context, IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task<List<UniversityDto>> GetUniversities(string? name = null, string? location = null)
    {
        var universities = await _universityRepository.GetAllUniversityAsync();
 
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

        public async Task<UniversityDto> GetUniversitiy(Guid id)
        {
            var universities = await _universityRepository.GetUniversityAsync(id);
            return universities;
        }

        public async Task<UniversityDto> CreateUniversity(CreateUniversityDto createUniversityDto)
        {
            var university = new University()
            {
                Name = createUniversityDto.Name,
                Location = createUniversityDto.Location
            };

            await _universityRepository.CreateUniversityAsync(university);

            var universityDto = new UniversityDto()
            {
                Id = university.Id,
                Name = university.Name,
                Location = university.Location
            };

            return universityDto;
        }

        public async Task<bool> UniversityExists(string name)
        {
            var university = await _universityRepository.UniversityExistsAsync(name);
            if (university != null)
                return true;
            return false;
        }

        public async Task<University> UpdateUniversitiy(Guid id, CreateUniversityDto updateUniversityDto)
        {
            var university = await _universityRepository.FindUniversityAsync(id);

            if (university == null)
            {
                return null;
            }

            university.Name = updateUniversityDto.Name;
            university.Location = updateUniversityDto.Location;

            await _universityRepository.UpdateUniversityAsync(id,university);

            return university;
        }

        public async Task<University> DeleteUniversitiy(Guid id)
        {
            var university = await _universityRepository.FindUniversityAsync(id);
            
            if (university == null)
            {
                return null;
            }
            
            await _universityRepository.DeleteUniversityAsync(university);
            return university;
        }
}