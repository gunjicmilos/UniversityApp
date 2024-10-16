using Microsoft.AspNetCore.Mvc;
using UniversityApp.Services.Interfaces;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamPeriodController : ControllerBase
    {
        private readonly IFacultyService _facultyService;
        private readonly IExamPeriodService _examPeriodService;
        private readonly ILogger<ExamPeriodController> _logger;

        public ExamPeriodController(IExamPeriodService examPeriodService, IFacultyService facultyService, ILogger<ExamPeriodController> logger)
        {
            _examPeriodService = examPeriodService;
            _facultyService = facultyService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamPeriodDto>>> GetExamPeriods()
        {
            try
            {
                var examPeriods = await _examPeriodService.GetExamPeriodsAsync();
                return Ok(examPeriods);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching universities.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExamPeriodDto>> GetExamPeriod(Guid id)
        {
            try
            {
                var examPeriod = await _examPeriodService.GetExamPeriodByIdAsync(id);

                if (examPeriod == null)
                {
                    return NotFound($"Exam period with id : {id} not found");
                }

                return Ok(examPeriod);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching universities.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        //[Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<ActionResult<ExamDto>> CreateExamPeriod(CreateExamPeriodDto createExamPeriodDto)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching universities.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        //[Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateExamPerion(Guid id, CreateExamPeriodDto updateExamPeriodDto)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching universities.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        //[Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExamPerion(Guid id)
        {
            try
            {
                var examPeriod = await _examPeriodService.DeleteExamPeriod(id);

                if (examPeriod == null)
                {
                    return NotFound($"Exam period with id : {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching universities.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }
    }
}
