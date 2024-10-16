using UniversityApp.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Services.Interfaces;

public interface ISubjectService
{
    Task<IEnumerable<SubjectDto>> GetSubjects(string? name = null, Guid? departmentId = null);
    Task<SubjectDto> GetSubjectsById(Guid id);
    Task<SubjectDto> CreateSubject(CreateSubjectDto createSubjectDto);
    Task<Subject> UpdateSubject(Guid id, CreateSubjectDto updateSubjectDto);
    Task<Subject> DeleteSubject(Guid id);
}