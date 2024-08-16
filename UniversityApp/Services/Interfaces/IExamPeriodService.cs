using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services.Interfaces;

public interface IExamPeriodService
{
    Task<IEnumerable<ExamPeriodDto>> GetExamPeriodsAsync();
    Task<ExamPeriodDto> GetExamPeriodByIdAsync(Guid id);
    Task<ActionResult<ExamPeriodDto>> CreateExamPeriod(CreateExamPeriodDto createExamPeriodDto);
    Task<ActionResult<ExamPeriod>> UpdateExamPeriod(Guid id, CreateExamPeriodDto updateExamPeriodDto);
    Task<ActionResult<ExamPeriod>> DeleteExamPeriod(Guid id);
}