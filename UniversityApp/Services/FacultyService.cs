using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Services;

public class FacultyService : IFacultyService
{
    private readonly DataContext _context;
    private readonly IFacultyRepository _facultyRepository;

    public FacultyService(DataContext context, IFacultyRepository facultyRepository)
    {
        _context = context;
        _facultyRepository = facultyRepository;
    }

        public async Task<ActionResult<IEnumerable<FacultyDto>>> GetFaculties([FromQuery] string? name = null, [FromQuery] string? location = null, [FromQuery] Guid? UniversityId = null)
        {
            var faculties = await _facultyRepository.GetAllFacultiesAsync();

            if (!string.IsNullOrWhiteSpace(name))
            {
                faculties = faculties.Where(f => f.Name.Contains(name)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                faculties = faculties.Where(f => f.Location.Contains(location)).ToList();
            }

            if (UniversityId != null)
            {
                faculties = faculties.Where(f => f.UniversityId == UniversityId).ToList();
            }

            return faculties;
        }

        public async Task<ActionResult<FacultyDto>> GetFaculty(Guid id)
        {
            return await _facultyRepository.GetFacultyByIdAsync(id);
        }

        public async Task<ActionResult<FacultyWithoutDepartmentsAndUsersDto>> CreateFaculty(CreateFacultyDto createFacultyDto)
        {
            var faculty = new Faculty()
            {
                Name = createFacultyDto.Name,
                Location = createFacultyDto.Location,
                UniversityId = createFacultyDto.UniversityId,
            };

            await _facultyRepository.AddFacultyAsync(faculty);

            var facultyDto = new FacultyWithoutDepartmentsAndUsersDto()
            {
                Id = faculty.Id,
                Name = faculty.Name,
                Location = faculty.Location,
            };

            return facultyDto;
        }

        public async Task<ActionResult<Faculty>> UpdateFaculty(Guid id, CreateFacultyDto updateFacultyDto)
        {
            var faculty = await _context.Faculties
                .Include(f => f.UserFaculties)
                .FirstOrDefaultAsync(f => f.Id == id);
            
            faculty.Name = updateFacultyDto.Name;
            faculty.Location = updateFacultyDto.Location;
            faculty.UniversityId = updateFacultyDto.UniversityId;

            faculty.UserFaculties.Clear();

            await _facultyRepository.UpdateFacultyAsync(faculty);

            return faculty;
        }

        

        public async Task<ActionResult<Faculty>> DeleteFaculty(Guid id)
        {
            var faculty = await _context.Faculties
                .Include(f => f.UserFaculties)
                .FirstOrDefaultAsync(f => f.Id == id);

            await _facultyRepository.DeleteFacultyAsync(id);

            return faculty;
        }

        public async Task<ActionResult<User>> AddUserToFaculty(AssignUserDto addUserDto)
        {
             return await _facultyRepository.AddUserToFacultyAsync(addUserDto);
        }

        public async Task<ActionResult<Faculty>> DeleteUserFromFaculty(AssignUserDto removeUserDto)
        {
            return await _facultyRepository.RemoveUserFromFacultyAsync(removeUserDto);
        }
}