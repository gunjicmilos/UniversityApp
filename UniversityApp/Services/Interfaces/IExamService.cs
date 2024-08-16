using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services.Interfaces;

public interface IExamService
{
    Task<ActionResult<IEnumerable<ExamDto>>> GetExams();
    Task<ActionResult<ExamDto>> GetExam(Guid id);
    Task<ActionResult<ExamDto>> CreateExam(CreateExamDto createExamDto);
    Task<ActionResult<Exam>> UpdateExam(Guid id, CreateExamDto updateExamDto);
    Task<ActionResult<Exam>> DeleteExam(Guid id);
}