using UniversityApp.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository.IRepository;

public interface IGradeRepository
{
    Task<StudentSubjectGrade> GetGrade(GetGradeDto getGradeDto);
    Task AddGradeAsync(StudentSubjectGrade createGrade); 
}