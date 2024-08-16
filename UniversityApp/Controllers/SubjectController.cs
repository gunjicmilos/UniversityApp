using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjects([FromQuery] string? name = null, [FromQuery] Guid? departmentId = null)
        {
            var subjects = await _subjectService.GetSubjects();
            return Ok(subjects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectDto>> GetSubjectsById(Guid id)
        {
            var subjects = await _subjectService.GetSubjectsById(id);
            return Ok(subjects);
        }

        [HttpPost]
        public async Task<ActionResult<SubjectDto>> CreateSubject(CreateSubjectDto createSubjectDto)
        {
            var subject = await _subjectService.CreateSubject(createSubjectDto);

            return Ok(subject);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSubject(Guid id, CreateSubjectDto updateSubjectDto)
        {
            var subject = await _subjectService.UpdateSubject(id, updateSubjectDto);

            if (subject == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubject(Guid id)
        {
            var subject = await _subjectService.DeleteSubject(id);

            if (subject == null)
            {
                return NotFound();
            }
            
            return NoContent();
        }
    }
}