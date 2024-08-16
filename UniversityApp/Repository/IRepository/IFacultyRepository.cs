using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository.IRepository;

public interface IFacultyRepository
{
    Task<List<FacultyDto>> GetAllFacultiesAsync();
    Task<FacultyDto> GetFacultyByIdAsync(Guid id);
    Task AddFacultyAsync(Faculty faculty);
    Task UpdateFacultyAsync(Faculty faculty);
    Task DeleteFacultyAsync(Guid id);
    Task<User> AddUserToFacultyAsync(AssignUserDto assignUserDto);
    Task<Faculty> RemoveUserFromFacultyAsync(AssignUserDto assignUserDto);
}