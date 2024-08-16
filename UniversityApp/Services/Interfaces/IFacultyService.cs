using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services.Interfaces;

public interface IFacultyService
{
    Task<ActionResult<IEnumerable<FacultyDto>>> GetFaculties(string? name = null, string? location = null, Guid? UniversityId = null);
    Task<ActionResult<FacultyDto>> GetFaculty(Guid id);
    Task<ActionResult<FacultyWithoutDepartmentsAndUsersDto>> CreateFaculty(CreateFacultyDto createFacultyDto);
    Task<ActionResult<Faculty>> UpdateFaculty(Guid id, CreateFacultyDto updateFacultyDto);
    Task<ActionResult<Faculty>> DeleteFaculty(Guid id);
    Task<ActionResult<User>> AddUserToFaculty(AssignUserDto addUserDto);
    Task<ActionResult<Faculty>> DeleteUserFromFaculty(AssignUserDto removeUserDto);
}