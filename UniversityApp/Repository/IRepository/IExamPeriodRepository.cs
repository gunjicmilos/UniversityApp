using UniversityApp.Models;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository.IRepository;

public interface IExamPeriodRepository
{
    Task<List<ExamPeriodDto>> GetAllExamPeriodsAsync();
    Task<ExamPeriodDto> GetExamPeriodByIdAsync(Guid id);
    Task<ExamPeriod> GetExamPeriodByIdFromDbAsync(Guid id);
    Task AddExamPeriodAsync(ExamPeriod examPeriod);
    Task UpdateExamPeriodAsync(ExamPeriod examPeriod);
    Task DeleteExamPeriodAsync(ExamPeriod examPeriod);
}