using UniversityApp.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Services.Interfaces;

public interface IExamPeriodService
{
    Task<List<ExamPeriodDto>> GetExamPeriodsAsync();
    Task<ExamPeriodDto> CreateExamPeriod(CreateExamPeriodDto createExamPeriodDto);
    Task<ExamPeriodDto> GetExamPeriodByIdAsync(Guid id);
    Task<ExamPeriod> UpdateExamPeriod(Guid id, CreateExamPeriodDto updateExamPeriodDto);
    Task<ExamPeriod> DeleteExamPeriod(Guid id);
}