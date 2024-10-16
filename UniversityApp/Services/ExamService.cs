using UniversityApp.Models;
using UniversityApp.Repository.IRepository;
using UniversityApp.Services.Interfaces;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Services;

public class ExamService : IExamService
{
    private readonly IExamRepository _examRepository;
    private readonly IUserRepository _userRepository;
    public ExamService(IExamRepository examRepository, IUserRepository userRepository)
    {
        _examRepository = examRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<ExamDto>> GetExams(string? name = null)
    {
        var exams = await _examRepository.GetAllExamsAsync();

        if (!string.IsNullOrWhiteSpace(name))
        {
            exams = exams.Where(u => u.Name.Contains(name)).ToList();
        }

        return exams;
    }

    public async Task<ExamDto> GetExam(Guid id)
    {
        return await _examRepository.GetExamByIdAsync(id);
    }

    public async Task<ExamDto> CreateExam(CreateExamDto createExamDto)
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
            var users = await _userRepository.GetUsersByUserIds(createExamDto);
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

    public async Task<Exam> UpdateExam(Guid Id, CreateExamDto updateExamDto)
    {
        var exam = await _examRepository.GetExamByIdFromDb(Id);

        if (exam == null)
        {
            return null;
        }
        exam.Name = updateExamDto.Name;
        exam.Date = updateExamDto.Date;
        exam.SubjectId = updateExamDto.SubjectId;
        exam.ExamPeriodId = updateExamDto.ExamPeriodId;

        exam.UserExams.Clear();
        if (updateExamDto.UserIds != null && updateExamDto.UserIds.Any())
        {
            var users = await _userRepository.GetUsersByUserIds(updateExamDto);
            exam.UserExams.AddRange(users.Select(u => new UserExam { UserId = u.Id, ExamId = exam.Id }));
        }

        await _examRepository.UpdateExamAsync(exam);

        return exam;
    }

    public async Task<Exam> DeleteExam(Guid Id)
    {
        var exam = await _examRepository.GetExamByIdFromDb(Id);

        await _examRepository.DeleteExamAsync(Id);

        return exam;
    }
}