using UniversityApp.Models;
using UniversityApp.Repository.IRepository;
using UniversityApp.Services.Interfaces;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Services;

public class GradeService : IGradeService
{
    private readonly IGradeRepository _gradeRepository;

    public GradeService(IGradeRepository gradeRepository)
    {
        _gradeRepository = gradeRepository;
    }

    public async Task<StudentSubjectGrade> CreateGradeAsync(CreateGradeDto createGradeDto)
    {
        var grade = new StudentSubjectGrade()
        {
            UserId = createGradeDto.UserId,
            SubjectId = createGradeDto.SubjectId,
            Grade = createGradeDto.Grade
        };
        await _gradeRepository.AddGradeAsync(grade);
        return grade;
    }

    public async Task<StudentSubjectGrade> GetGradeAsync(GetGradeDto getGradeDto)
    {
        return await _gradeRepository.GetGrade(getGradeDto);
    }
}