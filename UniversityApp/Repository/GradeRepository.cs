using Microsoft.EntityFrameworkCore;
using UniversityApp.Data;
using UniversityApp.Models;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository;

public class GradeRepository : IGradeRepository
{
    private readonly DataContext _context;

    public GradeRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<StudentSubjectGrade> GetGrade(GetGradeDto getGradeDto)
    {
        return await _context.StudentSubjectGrades.FirstOrDefaultAsync(s =>
            s.SubjectId == getGradeDto.SubjectId && s.UserId == getGradeDto.UserId);
    }
    
    public async Task AddGradeAsync(StudentSubjectGrade createGrade)
    {
         _context.StudentSubjectGrades.Add(createGrade);
         await _context.SaveChangesAsync();
    }
}