using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IFacultyRepository _facultyRepository;

        public DepartmentController(IDepartmentService departmentService, IFacultyRepository facultyRepository)
        {
            _departmentService = departmentService;
            _facultyRepository = facultyRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetDepartments([FromQuery] string? name = null, [FromQuery] Guid? facultyId = null)
        {
            return Ok(await _departmentService.GetDepartmentsAsync(name, facultyId));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetDepartment(Guid id)
        {
            var department = await _departmentService.GetDepartmentAsync(id);

            if (department == null)
            {
                return NotFound($"Department with id {id} not found");
            }

            return Ok(department);
        }

        //[Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> CreateDepartment(CreateDepartmentsDto createDepartmentsDto)
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

        //[Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Department>> UpdateDepartment(Guid id, CreateDepartmentsDto updateDepartmentsDto)
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

        //[Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartment(Guid id)
        {
            await _departmentService.DeleteDepartment(id);
            return NoContent();
        }
    }
}
