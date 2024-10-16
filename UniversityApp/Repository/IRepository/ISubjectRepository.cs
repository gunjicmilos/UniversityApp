using UniversityApp.Models;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository.IRepository;

public interface ISubjectRepository
{
    Task<List<SubjectDto>> GetAllSubjectsAsync();
    Task<SubjectDto> GetSubjectAsync(Guid id);
    Task<Subject> GetSubjectFromDbAsync(Guid id);
    Task CreateSubject(Subject subject);
    Task UpdateSubject(Subject subject);
    Task DeleteSubject(Subject subject);
}