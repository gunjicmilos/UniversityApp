using UniversityApp.Models;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Services.Interfaces;

public interface IFacultyService
{
    Task<IEnumerable<FacultyDto>> GetFaculties(string? name = null, string? location = null, Guid? UniversityId = null);
    Task<FacultyDto> GetFaculty(Guid id);
    Task<FacultyWithoutDepartmentsAndUsersDto> CreateFaculty(CreateFacultyDto createFacultyDto);
    Task<Faculty> UpdateFaculty(Guid id, CreateFacultyDto updateFacultyDto);
    Task<Faculty> DeleteFaculty(Guid id);
    Task<User> AddUserToFaculty(AssignUserDto addUserDto);
    Task<Faculty> DeleteUserFromFaculty(AssignUserDto removeUserDto);
    Task<bool> FacultyExists(string name);
    Task<bool> IsUserAddedToFaculty(AssignUserDto assignUserDto);
}