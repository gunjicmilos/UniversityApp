using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Services;

public class ExamService : IExamService
{
    private readonly DataContext _context;
    private readonly IExamRepository _examRepository;

    public ExamService(DataContext context, IExamRepository examRepository)
    {
        _context = context;
        _examRepository = examRepository;
    }

    public async Task<ActionResult<IEnumerable<ExamDto>>> GetExams()
    {
        return await _examRepository.GetAllExamsAsync();
    }

    public async Task<ActionResult<ExamDto>> GetExam(Guid id)
    {
        return await _examRepository.GetExamByIdAsync(id);
    }

    public async Task<ActionResult<ExamDto>> CreateExam(CreateExamDto createExamDto)
    {
        var exam = new Exam
        {
            Name = createExamDto.Name,
            Date = createExamDto.Date,
            SubjectId = createExamDto.SubjectId,
            ExamPeriodId = createExamDto.ExamPeriodId,
        };

        if (createExamDto.UserIds != null && createExamDto.UserIds.Any())
        {
            var users = await _context.Users.Where(u => createExamDto.UserIds.Contains(u.Id)).ToListAsync();
            exam.UserExams.AddRange(users.Select(u => new UserExam { UserId = u.Id, ExamId = exam.Id }));
        }

        await _examRepository.CreateExamAsync(exam);

        var examDto = new ExamDto
        {
            Id = exam.Id,
            Name = createExamDto.Name,
            Date = createExamDto.Date,
            SubjectId = createExamDto.SubjectId,
            ExamPeriodId = createExamDto.ExamPeriodId,
            UserIds = exam.UserExams.Select(ue => ue.UserId).ToList()
        };

        return examDto;
    }

    public async Task<ActionResult<Exam>> UpdateExam(Guid Id, CreateExamDto updateExamDto)
    {
        var exam = await _context.Exams.Include(e => e.UserExams).FirstOrDefaultAsync(e => e.Id == Id);
            
        exam.Name = updateExamDto.Name;
        exam.Date = updateExamDto.Date;
        exam.SubjectId = updateExamDto.SubjectId;
        exam.ExamPeriodId = updateExamDto.ExamPeriodId;

        exam.UserExams.Clear();
        if (updateExamDto.UserIds != null && updateExamDto.UserIds.Any())
        {
            var users = await _context.Users.Where(u => updateExamDto.UserIds.Contains(u.Id)).ToListAsync();
            exam.UserExams.AddRange(users.Select(u => new UserExam { UserId = u.Id, ExamId = exam.Id }));
        }

        await _examRepository.UpdateExamAsync(exam);

        return exam;
    }

    public async Task<ActionResult<Exam>> DeleteExam(Guid Id)
    {
        var exam = await _context.Exams.Include(e => e.UserExams).FirstOrDefaultAsync(e => e.Id == Id);

        await _examRepository.DeleteExamAsync(Id);

        return exam;
    }
}