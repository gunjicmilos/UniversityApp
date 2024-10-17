using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Models;
using UniversityApp.Repository.IRepository;
using UniversityApp.Services.Interfaces;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IFacultyRepository _facultyRepository;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(IDepartmentService departmentService, IFacultyRepository facultyRepository, ILogger<DepartmentController> logger)
        {
            _departmentService = departmentService;
            _facultyRepository = facultyRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetDepartments([FromQuery] string? name = null, [FromQuery] Guid? facultyId = null)
        {
            try
            {
                return Ok(await _departmentService.GetDepartmentsAsync(name, facultyId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching departments.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetDepartment(Guid id)
        {
            try
            {
                var department = await _departmentService.GetDepartmentAsync(id);

                if (department == null)
                {
                    return NotFound($"Department with id {id} not found");
                }

                return Ok(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching departments.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<ActionResult> CreateDepartment(CreateDepartmentsDto createDepartmentsDto)
        {
            try
            {
                var departmentExists =
                    await _departmentService.DepartmentExistsInFaculty(createDepartmentsDto.Name,
                        createDepartmentsDto.FacultyId);

                if (departmentExists)
                {
                    return BadRequest($"Department with name : {createDepartmentsDto.Name} already exists on faculty");
                }

                var department = await _departmentService.CreateDepartmentAsync(createDepartmentsDto);
                return Ok(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating departments.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Department>> UpdateDepartment(Guid id, CreateDepartmentsDto updateDepartmentsDto)
        {
            try
            {
                var faculty = await _facultyRepository.GetFacultyByIdAsync(updateDepartmentsDto.FacultyId);
                if (faculty == null)
                {
                    return NotFound($"Faculty with id : {updateDepartmentsDto.FacultyId} does not exists");
                }

                var departmentExists =
                    await _departmentService.DepartmentExistsInFaculty(updateDepartmentsDto.Name,
                        updateDepartmentsDto.FacultyId);

                if (departmentExists)
                {
                    return BadRequest($"Department with name : {updateDepartmentsDto.Name} already exists on faculty");
                }

                var department = await _departmentService.UpdateDepartmentAsync(id, updateDepartmentsDto);
                if (department == null)
                {
                    return NotFound($"Department with id : {id} already exists on faculty");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating departments.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartment(Guid id)
        {
            try
            {
                await _departmentService.DeleteDepartment(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleteing departments.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }
    }
}
