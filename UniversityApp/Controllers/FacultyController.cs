﻿using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class FacultyController : ControllerBase
    {
        private readonly IFacultyService _facultyService;

        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacultyDto>>> GetFaculties([FromQuery] string? name = null, [FromQuery] string? location = null, [FromQuery] Guid? UniversityId = null)
        {
            var faculties = await _facultyService.GetFaculties(name, location, UniversityId);
            return Ok(faculties);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FacultyDto>> GetFaculty(Guid id)
        {
            var faculties = await _facultyService.GetFaculty(id);

            if (faculties == null)
            {
                return NotFound($"Faculty with id {id} not found");
            }

            return Ok(faculties);
        }

        [HttpPost]
        public async Task<ActionResult<FacultyDto>> CreateFaculty(CreateFacultyDto createFacultyDto)
        {
            if (!await _facultyService.FacultyExists(createFacultyDto.Name))
            {
                var faculty = await _facultyService.CreateFaculty(createFacultyDto);
                return Ok(faculty);
            }
            else
            {
                return BadRequest($"Faculty {createFacultyDto.Name} already exists");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFaculty(Guid id, CreateFacultyDto updateFacultyDto)
        {
            var faculty = await _facultyService.UpdateFaculty(id, updateFacultyDto);

            if (faculty == null)
            {
                return NotFound($"Faculty with id {id} not found");
            }
            
            return NoContent();
        }

        

        [HttpDelete]
        public async Task<ActionResult> DeleteFaculty(Guid id)
        {
            var faculty = await _facultyService.DeleteFaculty(id);

            if (faculty == null)
            {
                return NotFound($"Faculty with id {id} not found");
            }

            return NoContent();
        }

        [HttpPost("/addUser")]
        public async Task<ActionResult> AddUserToFaculty(AssignUserDto addUserDto)
        {
            if (await _facultyService.IsUserAddedToFaculty(addUserDto))
            {
                return BadRequest("User is already added to faculty");
            }
            var faculty = await _facultyService.AddUserToFaculty(addUserDto);
            if (faculty == null)
            {
                return NotFound("Faculty or User does not exists");
            }

            return NoContent();
        }

        /*[HttpDelete("/removeUser")]
        public async Task<ActionResult> DeleteUserFromFaculty(AssignUserDto removeUserDto)
        {
            var faculty = await _facultyService.DeleteFaculty(removeUserDto);

            if (faculty == null)
            {
                return NotFound("Faculty doesnt not exists");
            }

            var userFaculty = faculty.UserFaculties
                .FirstOrDefault(uf => uf.UserId == removeUserDto.UserId);

            if (userFaculty == null)
            {
                return NotFound("User doesnt not exists on this faculty");
            }

            faculty.UserFaculties.Remove(userFaculty);
            await _context.SaveChangesAsync();

            return NoContent();*/
    }
}
