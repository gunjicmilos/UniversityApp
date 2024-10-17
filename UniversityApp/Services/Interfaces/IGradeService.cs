using UniversityApp.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Services.Interfaces;

public interface IGradeService
{
    Task<StudentSubjectGrade> CreateGradeAsync(CreateGradeDto createGradeDto);
    Task<StudentSubjectGrade> GetGradeAsync(GetGradeDto getGradeDto);
}