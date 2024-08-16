using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services.Interfaces;

public interface IUniversityService
{
    Task<ActionResult<IEnumerable<UniversityDto>>> GetUniversities(string? name = null, string? location = null);
    Task<ActionResult<UniversityDto>> GetUniversitiy(Guid id);
    Task<ActionResult<UniversityDto>> CreateUniversity(CreateUniversityDto createUniversityDto);
    Task<University> UpdateUniversitiy(Guid id, CreateUniversityDto updateUniversityDto);
    Task<University> DeleteUniversitiy(Guid id);
}