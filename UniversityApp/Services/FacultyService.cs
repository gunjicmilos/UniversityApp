using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services;

public class FacultyService
{
    private readonly DataContext _context;

    public FacultyService(DataContext context)
    {
        _context = context;
    }

        public async Task<ActionResult<IEnumerable<FacultyDto>>> GetFaculties([FromQuery] string? name = null, [FromQuery] string? location = null, [FromQuery] Guid? UniversityId = null)
        {
            var faculties = await _context.Faculties
                .Include(f => f.UserFaculties)
                .Select(f => new FacultyDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Location = f.Location,
                    UniversityId = f.UniversityId,
                    Departments = f.Departments.Select(d => new DepartmentDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        FacultyId = d.FacultyId,
                    }).ToList(),
                })
                .ToListAsync();

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
            var faculties = await _context.Faculties
                .Include(f => f.UserFaculties)
                .Select(f => new FacultyDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Location = f.Location,
                    UniversityId = f.UniversityId,
                    Departments = f.Departments.Select(d => new DepartmentDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        FacultyId = d.FacultyId,
                    }).ToList(),
                })
                .FirstOrDefaultAsync(f => f.Id == id);

            return faculties;
        }

        public async Task<ActionResult<FacultyWithoutDepartmentsAndUsersDto>> CreateFaculty(CreateFacultyDto createFacultyDto)
        {
            var faculty = new Faculty()
            {
                Name = createFacultyDto.Name,
                Location = createFacultyDto.Location,
                UniversityId = createFacultyDto.UniversityId,
            };

            _context.Faculties.Add(faculty);
            await _context.SaveChangesAsync();

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
            //faculty.UserFaculties = updateFacultyDto.UserIds.Select(userId => new UserFaculty
            //{
            //    UserId = userId,
            //    FacultyId = faculty.Id,
            //}).ToList();

            _context.Entry(faculty).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return faculty;
        }

        

        public async Task<ActionResult<Faculty>> DeleteFaculty(Guid id)
        {
            var faculty = await _context.Faculties
                .Include(f => f.UserFaculties)
                .FirstOrDefaultAsync(f => f.Id == id);
            
            _context.Faculties.Remove(faculty);
            await _context.SaveChangesAsync();

            return faculty;
        }

        public async Task<ActionResult<Faculty>> AddUserToFaculty(AssignUserDto addUserDto)
        {
            var faculty = await _context.Faculties
                .Include(faculty => faculty.UserFaculties)
                .FirstOrDefaultAsync(f => f.Id == addUserDto.FacultyId);

            var user = await _context.Users
                .Include(u => u.UserFaculties)
                .FirstOrDefaultAsync(u => u.Id == addUserDto.UserId);

            var userFaculty = new UserFaculty()
            {
                UserId = addUserDto.UserId,
                FacultyId = addUserDto.FacultyId,
            };

            faculty.UserFaculties.Add(userFaculty);

            await _context.SaveChangesAsync();

            return faculty;
        }

        public async Task<ActionResult<Faculty>> DeleteUserFromFaculty(AssignUserDto removeUserDto)
        {
            var faculty = await _context.Faculties
                .Include(faculty => faculty.UserFaculties)
                .FirstOrDefaultAsync(f => f.Id == removeUserDto.FacultyId);
            
            var userFaculty = faculty.UserFaculties
                .FirstOrDefault(uf => uf.UserId == removeUserDto.UserId);

            faculty.UserFaculties.Remove(userFaculty);
            await _context.SaveChangesAsync();

            return faculty;

        }
}