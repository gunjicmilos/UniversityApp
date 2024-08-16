using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<ActionResult> GetDepartments([FromQuery] string? name = null, [FromQuery] Guid? facultyId = null)
        {
            return Ok(await _departmentService.GetDepartmentsAsync(name, facultyId));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetDepartment(Guid id)
        {
            var departments = await _departmentService.GetDepartmentAsync(id);

            if (departments == null)
            {
                NotFound();
            }

            return Ok(departments);
        }

        [HttpPost]
        public async Task<ActionResult> CreateDepartment(CreateDepartmentsDto createDepartmentsDto)
        {
            var department = await _departmentService.CreateDepartmentAsync(createDepartmentsDto);
            return Ok(department);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Department>> UpdateDepartment(Guid id, CreateDepartmentsDto updateDepartmentsDto)
        {
            var department = await _departmentService.UpdateDepartmentAsync(id, updateDepartmentsDto);
            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartment(Guid id)
        {
            await _departmentService.DeleteDepartment(id);
            return NoContent();
        }
    }
}
