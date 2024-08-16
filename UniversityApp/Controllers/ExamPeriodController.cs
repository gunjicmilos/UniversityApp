using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services;

namespace UniversityManagament.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamPeriodController : ControllerBase
    {

        private readonly ExamPeriodService _examPeriodService;

        public ExamPeriodController(ExamPeriodService examPeriodService)
        {
            _examPeriodService = examPeriodService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamPeriodDto>>> GetExamPeriods()
        {
            var examPeriods = await _examPeriodService.GetExamPeriods();

            return Ok(examPeriods);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExamPeriodDto>> GetExamPeriod(Guid id)
        {
            var examPeriod = await _examPeriodService.GetExamPeriod(id);

            if (examPeriod == null)
            {
                return NotFound();
            }

            return Ok(examPeriod);
        }

        [HttpPost]
        public async Task<ActionResult<ExamDto>> CreateExamPeriod(CreateExamPeriodDto createExamPeriodDto)
        {
            var examPeriod = await _examPeriodService.CreateExamPeriod(createExamPeriodDto);

            return Ok(examPeriod);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateExamPerion(Guid id, CreateExamPeriodDto updateExamPeriodDto)
        {
            var examPeriod = await _examPeriodService.UpdateExamPeriod(id, updateExamPeriodDto);

            if (examPeriod == null)
            {
                return NotFound();
            }
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExamPerion(Guid id)
        {
            var examPeriod = await _examPeriodService.DeleteExamPeriod(id);

            if (examPeriod == null)
            {
                return NotFound();
            }
            
            return NoContent();
        }
    }
}
