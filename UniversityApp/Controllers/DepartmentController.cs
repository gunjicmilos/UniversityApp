using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services;

namespace UniversityManagament.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentService _departmentService;

        public DepartmentController(DepartmentService departmentService)
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
        public async Task<ActionResult> UpdateDepartment(Guid id, CreateDepartmentsDto updateDepartmentsDto)
        {
            var department = await _departmentService.UpdateDepartment(id, updateDepartmentsDto);
            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartment(Guid id)
        {
            var department = await _departmentService.DeleteDepartment(id);

            if (department == null)
            {
                return NotFound();
            }
            
            return Ok(department);
        }
    }
}
