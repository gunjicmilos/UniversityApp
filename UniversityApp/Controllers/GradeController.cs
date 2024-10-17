using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Services.Interfaces;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradeController : ControllerBase
{
    private readonly IGradeService _gradeService;
    private readonly ISubjectService _subjectService;
    private readonly IUserService _userService;
    private readonly ILogger<GradeController> _logger;

    public GradeController(IGradeService gradeService, ISubjectService subjectService, IUserService userService, ILogger<GradeController> logger)
    {
        _gradeService = gradeService;
        _subjectService = subjectService;
        _userService = userService;
        _logger = logger;
    }
    
    
    [Authorize(Policy = "ProfessorPolicy")]
    [HttpPost]
    public async Task<ActionResult> CreateGrade(CreateGradeDto createGradeDto)
    {
        try
        {
            if (await _userService.GetUserAsync(createGradeDto.UserId) == null)
            {
                return BadRequest("User not found");
            }

            if (await _subjectService.GetSubjectsById(createGradeDto.SubjectId) == null)
            {
                return BadRequest("Subject not found");
            }

            await _gradeService.CreateGradeAsync(createGradeDto);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding grades.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    [Authorize(Policy = "UserPolicy")]
    [HttpPost("api/grade")]
    public async Task<ActionResult> GetGrade(GetGradeDto getGradeDto)
    {
        try
        {
            if (await _userService.GetUserAsync(getGradeDto.UserId) == null)
            {
                return BadRequest("User not found");
            }

            if (await _subjectService.GetSubjectsById(getGradeDto.SubjectId) == null)
            {
                return BadRequest("Subject not found");
            }

            var result = await _gradeService.GetGradeAsync(getGradeDto);
            return Ok(result.Grade);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching grades.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}