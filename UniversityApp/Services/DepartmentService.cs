using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services;

public class DepartmentService
{
    private readonly DataContext _context;

    public DepartmentService(DataContext context)
    {
        _context = context;
    }

    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartmentsAsync([FromQuery] string? name = null,
        [FromQuery] Guid? facultyId = null)
    {
        var departments = await _context.Departments
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                FacultyId = d.FacultyId
            })
            .ToListAsync();

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

    public async Task<ActionResult<DepartmentDto>> GetDepartmentAsync(Guid id)
    {
        var department = await _context.Departments
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                FacultyId = d.FacultyId
            })
            .FirstOrDefaultAsync(d => d.Id == id);

        return department;
    }
    
    public async Task<ActionResult<DepartmentDto>> CreateDepartmentAsync(CreateDepartmentsDto createDepartmentsDto)
    {
        var department = new Department()
        {
            Name = createDepartmentsDto.Name,
            FacultyId = createDepartmentsDto.FacultyId,
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        var departmentsDto = new DepartmentDto()
        {
            Id = department.Id,
            Name = department.Name,
            FacultyId = department.FacultyId,
        };

        return departmentsDto;
    }
    
    public async Task<ActionResult<Department>> UpdateDepartment(Guid id, CreateDepartmentsDto updateDepartmentsDto)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        
        department.Name = updateDepartmentsDto.Name;
        department.FacultyId = updateDepartmentsDto.FacultyId;

        _context.Entry(department).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return department;
    }

    public async Task<ActionResult<Department>> DeleteDepartment(Guid id)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        
        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();

        return department;
    }

}