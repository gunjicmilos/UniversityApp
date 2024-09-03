using Microsoft.EntityFrameworkCore;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository;

public class SubjectRepository : ISubjectRepository
{
    private readonly DataContext _context;

    public SubjectRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<SubjectDto>> GetAllSubjectsAsync()
    {
        return await _context.Subjects
            .Include(s => s.UserSubjects)
            .Select(s => new SubjectDto
            {
                Id = s.Id,
                Name = s.Name,
                DepartmentId = s.DepartmentId,
                Espb = s.Espb,
                UserIds = s.UserSubjects.Select(us => us.UserId).ToList()
            }).ToListAsync();
        ;
    }

    public async Task<SubjectDto> GetSubjectAsync(Guid id)
    {
        return await _context.Subjects
            .Include(s => s.UserSubjects)
            .Select(s => new SubjectDto
            {
                Id = s.Id,
                Name = s.Name,
                DepartmentId = s.DepartmentId,
                Espb = s.Espb,
                UserIds = s.UserSubjects.Select(us => us.UserId).ToList()
            })
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Subject> GetSubjectFromDbAsync(Guid id)
    {
       return await _context.Subjects.Include(s => s.UserSubjects).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task CreateSubject(Subject subject)
    {
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSubject(Subject subject)
    {
        _context.Entry(subject).State = EntityState.Modified;
        await _context.SaveChangesAsync(); 
    }

    public async Task DeleteSubject(Subject subject)
    {
        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync();
    }
}