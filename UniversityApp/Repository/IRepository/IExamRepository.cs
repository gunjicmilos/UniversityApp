using UniversityApp.Models;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository.IRepository;

public interface IExamRepository
{
    Task<List<ExamDto>> GetAllExamsAsync();
    Task<ExamDto> GetExamByIdAsync(Guid id);
    Task<Exam> GetExamByIdFromDb(Guid id);
    Task CreateExamAsync(Exam exam);
    Task UpdateExamAsync(Exam exam); 
    Task DeleteExamAsync(Guid id); 
}