using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services.Interfaces;

public interface IExamService
{
    Task<IEnumerable<ExamDto>> GetExams();
    Task<ExamDto> GetExam(Guid id);
    Task<ExamDto> CreateExam(CreateExamDto createExamDto);
    Task<Exam> UpdateExam(Guid id, CreateExamDto updateExamDto);
    Task<Exam> DeleteExam(Guid id);
}