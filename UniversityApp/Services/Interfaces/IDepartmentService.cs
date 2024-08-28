using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services.Interfaces;

public interface IDepartmentService
{
    Task<List<DepartmentDto>> GetDepartmentsAsync(string? name = null, Guid? facultyId = null);
    Task<DepartmentDto> GetDepartmentAsync(Guid id);
    Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentsDto createDepartmentsDto);
    Task<Department> UpdateDepartmentAsync(Guid id, CreateDepartmentsDto updateDepartmentDto);
    Task DeleteDepartment(Guid id);
}