using Microsoft.AspNetCore.Mvc;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentsRepository _departmentsRepository;

    public DepartmentService(IDepartmentsRepository departmentsRepository)
    {
        _departmentsRepository = departmentsRepository;
    }

    public async Task<List<DepartmentDto>> GetDepartmentsAsync([FromQuery] string? name = null,
        [FromQuery] Guid? facultyId = null)
    {
        var departments = await _departmentsRepository.GetDepartmentsAsync();

        if (!string.IsNullOrWhiteSpace(name))
        {
            departments = departments.Where(u => u.Name.Contains(name)).ToList();
        }

        if (facultyId != null)
        {
            departments = departments.Where(u => u.FacultyId == facultyId).ToList();
        }

        return departments;
    }

    public async Task<DepartmentDto> GetDepartmentAsync(Guid id)
    {
        var department = await _departmentsRepository.GetDepartmentByIdAsync(id);
        return department;
    }
    
    public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentsDto createDepartmentsDto)
    {
        var department = new Department()
        {
            Name = createDepartmentsDto.Name,
            FacultyId = createDepartmentsDto.FacultyId,
        };

        await _departmentsRepository.AddDepartmentAsync(department);

        var departmentsDto = new DepartmentDto()
        {
            Id = department.Id,
            Name = department.Name,
            FacultyId = department.FacultyId,
        };

        return departmentsDto;
    }
    public async Task<Department> UpdateDepartmentAsync(Guid id, CreateDepartmentsDto updateDepartmentDto)
    {
        return await _departmentsRepository.UpdateDepartmentAsync(id, updateDepartmentDto);
    }

    public Task DeleteDepartmentAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteDepartment(Guid id)
    {
        await _departmentsRepository.DeleteDepartmentAsync(id);
    }
}