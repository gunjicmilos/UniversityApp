using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository.IRepository;

public interface IUniversityRepository
{
    Task<List<UniversityDto>> GetAllUniversityAsync();
    Task<UniversityDto> GetUniversityAsync(Guid id);
    Task CreateUniversityAsync(University university);
    Task<University> UniversityExistsAsync(string name);
    Task<University> FindUniversityAsync(Guid id);
    Task UpdateUniversityAsync(Guid id,University updateUniversityDto);
    Task DeleteUniversityAsync(University university);
}