using Microsoft.EntityFrameworkCore;
using UniversityApp.Data;
using UniversityApp.Models;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository;

public class ExamPeriodRepository : IExamPeriodRepository
{
    private readonly DataContext _context;

    public ExamPeriodRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<ExamPeriodDto>> GetAllExamPeriodsAsync()
    {
        return await _context.ExamPeriods
            .Select(ep => new ExamPeriodDto
            {
                Id = ep.Id,
                Name = ep.Name,
                StartDate = ep.StartDate,
                EndData = ep.EndDate,
                FacultyId = ep.FacultyId,
            }).ToListAsync();
    }
    
    public async Task<ExamPeriodDto> GetExamPeriodByIdAsync(Guid id)
    {
        return await _context.ExamPeriods
            .Where(ep => ep.Id == id)
            .Select(ep => new ExamPeriodDto
            {
                Id = ep.Id,
                Name = ep.Name,
                StartDate = ep.StartDate,
                EndData = ep.EndDate, // Proveri da li je ovo tačan naziv
                FacultyId = ep.FacultyId,
            })
            .FirstOrDefaultAsync();
    }
    
    public async Task<ExamPeriod> GetExamPeriodByIdFromDbAsync(Guid id)
    {
        return await _context.ExamPeriods
            .Where(ep => ep.Id == id)
            .Select(ep => new ExamPeriod
            {
                Id = ep.Id,
                Name = ep.Name,
                StartDate = ep.StartDate,
                FacultyId = ep.FacultyId,
            })
            .FirstOrDefaultAsync();
    }
    
    public async Task AddExamPeriodAsync(ExamPeriod examPeriod)
    {
        _context.ExamPeriods.Add(examPeriod);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateExamPeriodAsync(ExamPeriod examPeriod)
    {
        _context.Entry(examPeriod).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteExamPeriodAsync(ExamPeriod examPeriod)
    {
        _context.ExamPeriods.Remove(examPeriod);
        await _context.SaveChangesAsync();
    }
}