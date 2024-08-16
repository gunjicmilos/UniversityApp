using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository.IRepository;

public interface IExamPeriodRepository
{
    Task<IEnumerable<ExamPeriodDto>> GetAllExamPeriodsAsync();
    Task<ExamPeriodDto> GetExamPeriodByIdAsync(Guid id);
    Task AddExamPeriodAsync(ExamPeriod examPeriod);
    Task UpdateExamPeriodAsync(ExamPeriod examPeriod);
    Task DeleteExamPeriodAsync(ExamPeriod examPeriod);
}