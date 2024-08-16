using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamDto>>> GetExams()
        {
            var exams = await _examService.GetExams();

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

        [HttpPost]
        public async Task<ActionResult<ExamDto>> CreateExam(CreateExamDto createExamDto)
        {
            var exam = await _examService.CreateExam(createExamDto);

            return Ok(exam);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> updateExam(Guid Id, CreateExamDto updateExamDto)
        {
            var exam = await _examService.UpdateExam(Id, updateExamDto);

            if (exam == null)
            {
                return NotFound();
            }
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteExam(Guid id)
        {
            var exam = await _examService.DeleteExam(id);

            if (exam == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
