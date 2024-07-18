using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services;

public class ExamService
{
    private readonly DataContext _context;

    public ExamService(DataContext context)
    {
        _context = context;
    }

    public async Task<ActionResult<IEnumerable<ExamDto>>> GetExams()
        {
            var exams = await _context.Exams
                .Include(e => e.UserExams)
                .Select(e => new ExamDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Date = e.Date,
                    SubjectId = e.SubjectId,
                    ExamPeriodId = e.ExamPeriodId,
                    UserIds = e.UserExams.Select(ue => ue.UserId).ToList()
                })
                .ToListAsync();

            return exams;
        }

        public async Task<ActionResult<ExamDto>> GetExam(Guid id)
        {
            var exam = await _context.Exams
                .Include(e => e.UserExams)
                .Select(e => new ExamDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Date = e.Date,
                    SubjectId = e.SubjectId,
                    ExamPeriodId = e.ExamPeriodId,
                    UserIds = e.UserExams.Select(ue => ue.UserId).ToList()
                })
                .FirstOrDefaultAsync(e => e.Id == id);
            
            return exam;
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

            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();

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

        public async Task<ActionResult<Exam>> updateExam(Guid Id, CreateExamDto updateExamDto)
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

            _context.Entry(exam).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return exam;
        }

        public async Task<ActionResult<Exam>> deleteExam(Guid Id)
        {
            var exam = await _context.Exams.Include(e => e.UserExams).FirstOrDefaultAsync(e => e.Id == Id);
            
            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();

            return exam;
        }
}