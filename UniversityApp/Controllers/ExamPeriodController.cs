using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamPeriodController : ControllerBase
    {
        private readonly IFacultyService _facultyService;
        private readonly IExamPeriodService _examPeriodService;

        public ExamPeriodController(IExamPeriodService examPeriodService, IFacultyService facultyService)
        {
            _examPeriodService = examPeriodService;
            _facultyService = facultyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamPeriodDto>>> GetExamPeriods()
        {
            var examPeriods = await _examPeriodService.GetExamPeriodsAsync();

            return Ok(examPeriods);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExamPeriodDto>> GetExamPeriod(Guid id)
        {
            var examPeriod = await _examPeriodService.GetExamPeriodByIdAsync(id);

            if (examPeriod == null)
            {
                return NotFound($"Exam period with id : {id} not found");
            }

            return Ok(examPeriod);
        }

        [HttpPost]
        public async Task<ActionResult<ExamDto>> CreateExamPeriod(CreateExamPeriodDto createExamPeriodDto)
        {
            
            var faculty = await _facultyService.GetFaculty(createExamPeriodDto.FacultyId);
            if (faculty == null)
            {
                return NotFound($"Faculty with id : {createExamPeriodDto.FacultyId} not found");
            }
            var examPeriods = await _examPeriodService.GetExamPeriodsAsync();
            if (examPeriods.Any(ep =>
                    ep.FacultyId == createExamPeriodDto.FacultyId && ep.Name == createExamPeriodDto.Name))
            {
                return BadRequest($"Exam period already exists on faculty");
            }
            
            var examPeriod = await _examPeriodService.CreateExamPeriod(createExamPeriodDto);

            return Ok(examPeriod);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateExamPerion(Guid id, CreateExamPeriodDto updateExamPeriodDto)
        {
            var faculty = await _facultyService.GetFaculty(updateExamPeriodDto.FacultyId);
            if (faculty == null)
            {
                return NotFound($"Faculty with id : {updateExamPeriodDto.FacultyId} not found");
            }
            var examPeriods = await _examPeriodService.GetExamPeriodsAsync();
            if (examPeriods.Any(ep =>
                    ep.FacultyId == updateExamPeriodDto.FacultyId && ep.Name == updateExamPeriodDto.Name))
            {
                return BadRequest($"Exam period already exists on faculty");
            }
            
            var examPeriod = await _examPeriodService.UpdateExamPeriod(id, updateExamPeriodDto);

            if (examPeriod == null)
            {
                return NotFound($"Exam period with id : {id} not found");
            }
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExamPerion(Guid id)
        {
            var examPeriod = await _examPeriodService.DeleteExamPeriod(id);

            if (examPeriod == null)
            {
                return NotFound($"Exam period with id : {id} not found");
            }
            
            return NoContent();
        }
    }
}
