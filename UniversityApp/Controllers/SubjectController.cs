﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Models;
using UniversityApp.Services.Interfaces;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly IDepartmentService _departmentService;
        private readonly IUserService _userService;
        private readonly ILogger<SubjectController> _logger;

        public SubjectController(ISubjectService subjectService, IDepartmentService departmentService, IUserService userService, ILogger<SubjectController> logger)
        {
            _subjectService = subjectService;
            _departmentService = departmentService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjects([FromQuery] string? name = null, [FromQuery] Guid? departmentId = null)
        {
            try
            {
                var subjects = await _subjectService.GetSubjects();
                return Ok(subjects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching subjects.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectDto>> GetSubjectsById(Guid id)
        {
            try
            {
                var subject = await _subjectService.GetSubjectsById(id);

                if (subject == null)
                {
                    return NotFound($"Subject with id : {id} not found");
                }

                return Ok(subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching subjects.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [Authorize(Policy = "ProfessorPolicy")]
        [HttpPost]
        public async Task<ActionResult<SubjectDto>> CreateSubject(CreateSubjectDto createSubjectDto)
        {
            try{
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating subjects.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [Authorize(Policy = "ProfessorPolicy")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSubject(Guid id, CreateSubjectDto updateSubjectDto)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating subjects.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [Authorize(Policy = "ProfessorPolicy")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubject(Guid id)
        {
            try
            {
                var subject = await _subjectService.DeleteSubject(id);

                if (subject == null)
                {
                    return NotFound($"Subject with id : {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting subjects.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [Authorize(Policy = "ProfessorPolicy")]
        [HttpPost("/addUserToSubject")]
        public async Task<ActionResult> AddUserToSubject(AddUserToSubjectDto addUserToSubjectDto)
        {
            try
            {
                if (await _subjectService.GetSubjectsById(addUserToSubjectDto.SubjectId) == null)
                {
                    return BadRequest("Subject Not Found");
                }

                if (await _userService.GetUserAsync(addUserToSubjectDto.UserId) == null)
                {
                    return BadRequest("User Not Found");
                }

                var userSubject = new UserSubject()
                {
                    UserId = addUserToSubjectDto.UserId,
                    SubjectId = addUserToSubjectDto.SubjectId
                };
                await _subjectService.AddUserToSubject(userSubject);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user to subject.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }
        
        [Authorize(Policy = "UserPolicy")]
        [HttpPost("/getSubjectsOfUser/{id}")]
        public async Task<ActionResult> GetSubjectsOfUser(Guid id)
        {
            try
            {
                if (await _userService.GetUserAsync(id) == null)
                {
                    return BadRequest("User Not Found");
                }

                var result = await _subjectService.GetSubjects(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching subjects.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }
    }
}