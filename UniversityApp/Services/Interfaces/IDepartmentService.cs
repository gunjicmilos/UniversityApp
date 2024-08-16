using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services.Interfaces;

public interface IDepartmentService
{
    Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartmentsAsync(string? name = null, Guid? facultyId = null);
    Task<ActionResult<DepartmentDto>> GetDepartmentAsync(Guid id);
    Task<ActionResult<DepartmentDto>> CreateDepartmentAsync(CreateDepartmentsDto createDepartmentsDto);
    Task<Department> UpdateDepartmentAsync(Guid id, CreateDepartmentsDto updateDepartmentDto);
    Task DeleteDepartment(Guid id);
}