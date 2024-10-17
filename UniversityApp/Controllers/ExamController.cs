using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Services.Interfaces;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;
        private readonly ISubjectService _subjectService;
        private readonly IExamPeriodService _examPeriodService;
        private readonly IUserService _userService;
        private readonly ILogger<ExamController> _logger;

        public ExamController(IExamService examService, ISubjectService subjectService, IExamPeriodService examPeriodService, IUserService userService, ILogger<ExamController> logger)
        {
            _examService = examService;
            _subjectService = subjectService;
            _examPeriodService = examPeriodService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamDto>>> GetExams([FromQuery] string? name = null)
        {
            try
            {
                var exams = await _examService.GetExams(name);
                return Ok(exams);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching exams.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExamDto>> GetExam(Guid id)
        {
            try
            {
                var exam = await _examService.GetExam(id);

                if (exam == null)
                {
                    return NotFound();
                }

                return Ok(exam);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching universities.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<ActionResult<ExamDto>> CreateExam(CreateExamDto createExamDto)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creting exams.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateExam(Guid Id, CreateExamDto updateExamDto)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating exams.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteExam(Guid id)
        {
            try
            {
                var exam = await _examService.DeleteExam(id);

                if (exam == null)
                {
                    return NotFound($"Exam with id : {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting exams.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }
    }
}
