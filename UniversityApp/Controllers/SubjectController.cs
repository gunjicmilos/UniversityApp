using Microsoft.AspNetCore.Mvc;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly IDepartmentService _departmentService;
        private readonly IUserService _userService;

        public SubjectController(ISubjectService subjectService, IDepartmentService departmentService, IUserService userService)
        {
            _subjectService = subjectService;
            _departmentService = departmentService;
            _userService = userService;
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
            var subject = await _subjectService.GetSubjectsById(id);

            if (subject == null)
            {
                return NotFound($"Subject with id : {id} not found");
            }
            
            return Ok(subject);
        }

        [HttpPost]
        public async Task<ActionResult<SubjectDto>> CreateSubject(CreateSubjectDto createSubjectDto)
        {
            var departments = await _departmentService.GetDepartmentsAsync();
            var department = departments.FirstOrDefault(d => d.Id == createSubjectDto.DepartmentId);
            
            if (department == null)
            {
                return NotFound($"Department with id : {createSubjectDto.DepartmentId} not found");
            }

            if (department.Subjects.Any(s => s.Name == createSubjectDto.Name))
            {
                return BadRequest($"Subject with name : {createSubjectDto.Name} already exists in department");
            }            
            foreach (var userId in createSubjectDto.UsersIds)
            {
                var user = await _userService.GetUserAsync(userId);
                if (user == null)
                {
                    return NotFound($"User with id : {userId} not found");
                }
            }
            var subject = await _subjectService.CreateSubject(createSubjectDto);

            return Ok(subject);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSubject(Guid id, CreateSubjectDto updateSubjectDto)
        {
            var subjectExists = await _subjectService.GetSubjectsById(id);
            if (subjectExists == null)
            {
                return NotFound($"Subject with name : {updateSubjectDto.Name} does not exists");
            }
            var departments = await _departmentService.GetDepartmentsAsync();
            var department = departments.FirstOrDefault(d => d.Id == updateSubjectDto.DepartmentId);
            
            if (department == null)
            {
                return NotFound($"Department with id : {updateSubjectDto.DepartmentId} not found");
            }

            if (department.Subjects.Any(s => s.Name == updateSubjectDto.Name))
            {
                return BadRequest($"Subject with name : {updateSubjectDto.Name} already exists in department");
            }            
            foreach (var userId in updateSubjectDto.UsersIds)
            {
                var user = await _userService.GetUserAsync(userId);
                if (user == null)
                {
                    return NotFound($"User with id : {userId} not found");
                }
            }

            var subject = await _subjectService.UpdateSubject(id, updateSubjectDto);

            if (subject == null)
            {
                return NotFound($"Subject with id : {id} not found");
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