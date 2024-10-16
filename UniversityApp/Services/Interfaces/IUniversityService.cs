using Microsoft.AspNetCore.Mvc;
using UniversityApp.Models;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services.Interfaces;

public interface IUniversityService
{
    Task<List<UniversityDto>> GetUniversities(string? name = null, string? location = null);
    Task<UniversityDto> GetUniversitiy(Guid id);
    Task<UniversityDto> CreateUniversity(CreateUniversityDto createUniversityDto);
    Task<University> UpdateUniversitiy(Guid id, CreateUniversityDto updateUniversityDto);
    Task<University> DeleteUniversitiy(Guid id);
    Task<bool> UniversityExists(string name);
}