using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;
        private readonly ISubjectService _subjectService;
        private readonly IExamPeriodService _examPeriodService;
        private readonly IUserService _userService;

        public ExamController(IExamService examService, ISubjectService subjectService, IExamPeriodService examPeriodService, IUserService userService)
        {
            _examService = examService;
            _subjectService = subjectService;
            _examPeriodService = examPeriodService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamDto>>> GetExams([FromQuery] string? name = null)
        {
            var exams = await _examService.GetExams(name);

            return Ok(exams);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExamDto>> GetExam(Guid id)
        {
            var exam = await _examService.GetExam(id);

            if (exam == null)
            {
                return NotFound();
            }

            return Ok(exam);
        }

        //[Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<ActionResult<ExamDto>> CreateExam(CreateExamDto createExamDto)
        {
            var subject = await _subjectService.GetSubjectsById(createExamDto.SubjectId);
            if (subject == null)
            {
                return NotFound($"Subject with id : {createExamDto.SubjectId} not found");
            }

            var examPeriod = await _examPeriodService.GetExamPeriodByIdAsync(createExamDto.ExamPeriodId);
            if (examPeriod == null)
            {
                return NotFound($"Exam Perod with id : {createExamDto.ExamPeriodId} not found");
            }
            
            foreach (var userId in createExamDto.UserIds)
            {
                var user = await _userService.GetUserAsync(userId);
                if (user == null)
                {
                    return NotFound($"User with id : {userId} not found");
                }
            }
            
            var exams = await _examService.GetExams();
            if (exams.Any(e =>
                    e.ExamPeriodId == createExamDto.ExamPeriodId && e.Name == createExamDto.Name))
            {
                return BadRequest($"Exam already exists for this Exam Period");
            }
            
            var exam = await _examService.CreateExam(createExamDto);

            return Ok(exam);
        }

        //[Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateExam(Guid Id, CreateExamDto updateExamDto)
        {
            var subject = await _subjectService.GetSubjectsById(updateExamDto.SubjectId);
            if (subject == null)
            {
                return NotFound($"Subject with id : {updateExamDto.SubjectId} not found");
            }

            var examPeriod = await _examPeriodService.GetExamPeriodByIdAsync(updateExamDto.ExamPeriodId);
            if (examPeriod == null)
            {
                return NotFound($"Exam Perod with id : {updateExamDto.ExamPeriodId} not found");
            }
            
            foreach (var userId in updateExamDto.UserIds)
            {
                var user = await _userService.GetUserAsync(userId);
                if (user == null)
                {
                    return NotFound($"User with id : {userId} not found");
                }
            }
            
            var exams = await _examService.GetExams();
            if (exams.Any(e =>
                    e.ExamPeriodId == updateExamDto.ExamPeriodId && e.Name == updateExamDto.Name))
            {
                return BadRequest($"Exam already exists for this Exam Period");
            }
            
            var exam = await _examService.UpdateExam(Id, updateExamDto);

            if (exam == null)
            {
                return NotFound($"Exam with id : {Id} not found");
            }
            
            return NoContent();
        }

        //[Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteExam(Guid id)
        {
            var exam = await _examService.DeleteExam(id);

            if (exam == null)
            {
                return NotFound($"Exam with id : {id} not found");
            }

            return NoContent();
        }
    }
}
