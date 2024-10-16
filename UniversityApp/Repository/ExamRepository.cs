using Microsoft.EntityFrameworkCore;
using UniversityApp.Data;
using UniversityApp.Models;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository;

public class ExamRepository : IExamRepository
{ 
    private readonly DataContext _context;

    public ExamRepository(DataContext context)
    {
        _context = context;
    }
    
    public async Task<List<ExamDto>> GetAllExamsAsync()
    {
        var exams = await _context.Exams
            .Include(e => e.UserExams)
            .Select(e => new ExamDto
            {
                Id = e.Id,
                Name = e.Name,
                Date = e.Date,
                SubjectId = e.SubjectId,
                ExamPeriodId = e.ExamPeriodId,
            })
            .ToListAsync();

        return exams;
    }
    
    public async Task<ExamDto> GetExamByIdAsync(Guid id)
    {
        var exam = await _context.Exams
            .Include(e => e.UserExams)
            .Select(e => new ExamDto
            {
                Id = e.Id,
                Name = e.Name,
                Date = e.Date,
                SubjectId = e.SubjectId,
                ExamPeriodId = e.ExamPeriodId,
            })
            .FirstOrDefaultAsync(e => e.Id == id);
            
        return exam;
    }

    public async Task<Exam> GetExamByIdFromDb(Guid id)
    {
        return await _context.Exams.Include(e => e.UserExams).FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Exam> GetExamByIdFromDbAsync(Guid id)
    {
        var exam = await _context.Exams
            .Include(e => e.UserExams)
            .Select(e => new Exam
            {
                Id = e.Id,
                Name = e.Name,
                Date = e.Date,
                SubjectId = e.SubjectId,
                ExamPeriodId = e.ExamPeriodId,
            })
            .FirstOrDefaultAsync(e => e.Id == id);
            
        return exam;
    }

    public async Task CreateExamAsync(Exam exam)
    {
        _context.Exams.Add(exam);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateExamAsync(Exam exam)
    {
        _context.Entry(exam).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteExamAsync(Guid id)
    {
        var exam = await _context.Exams.FindAsync(id);
        if (exam != null)
        {
            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();
        }
    }
}