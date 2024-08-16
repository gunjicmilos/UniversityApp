using Microsoft.EntityFrameworkCore;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository;

public class DepartmentsRepository : IDepartmentsRepository
{
    private readonly DataContext _context;

    public DepartmentsRepository(DataContext context)
    {
        _context = context;
    }
    
    public async Task<List<DepartmentDto>> GetDepartmentsAsync()
    {
        return await _context.Departments
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                FacultyId = d.FacultyId
            })
            .ToListAsync();
    }
    
    public async Task<DepartmentDto> GetDepartmentByIdAsync(Guid id)
    {
        return await _context.Departments
            .Where(d => d.Id == id)
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                FacultyId = d.FacultyId
            })
            .FirstOrDefaultAsync();
    }
    
    public async Task AddDepartmentAsync(Department department)
    {
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Department> UpdateDepartmentAsync(Guid id, CreateDepartmentsDto updateDepartmentDto)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        if (department != null)
        {
            department.Name = updateDepartmentDto.Name;
            department.FacultyId = updateDepartmentDto.FacultyId;

            _context.Entry(department).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        return department;
    }
    
    public async Task DeleteDepartmentAsync(Guid id)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        if (department != null)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}