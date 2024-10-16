using UniversityApp.Models;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository.IRepository;

public interface IDepartmentsRepository
{
    Task<List<Department>> GetDepartmentsAsync();
    Task<DepartmentDto> GetDepartmentByIdAsync(Guid id);
    Task<bool> DepartmentExistsInFacultyAsync(string name, Guid id);
    Task AddDepartmentAsync(Department department); 
    Task<Department> UpdateDepartmentAsync(Guid id, CreateDepartmentsDto updateDepartmentDto);
    Task DeleteDepartmentAsync(Guid id);
}