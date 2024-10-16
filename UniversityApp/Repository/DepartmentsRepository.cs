using Microsoft.EntityFrameworkCore;
using UniversityApp.Data;
using UniversityApp.Models;
using UniversityApp.Repository.IRepository;
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
    
    public async Task<List<Department>> GetDepartmentsAsync()
    {
        return await _context.Departments
            .Select(d => new Department
            {
                Id = d.Id,
                Name = d.Name,
                FacultyId = d.FacultyId,
                Subjects = d.Subjects
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

    public async Task<bool> DepartmentExistsInFacultyAsync(string name, Guid id)
    {
        var result = await _context.Faculties
            .FirstOrDefaultAsync(uf => uf.Id == id && uf.Departments.Any(d => d.Name == name));
        if (result == null)
        {
            return false;
        }
        return true;
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