using UniversityApp.Models;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository.IRepository;

public interface IFacultyRepository
{
    Task<List<FacultyDto>> GetAllFacultiesAsync();
    Task<FacultyDto> GetFacultyByIdAsync(Guid id);
    Task<Faculty> GetFacultyByIdFromDbAsync(Guid id);
    Task AddFacultyAsync(Faculty faculty);
    Task UpdateFacultyAsync(Faculty faculty);
    Task DeleteFacultyAsync(Guid id);
    Task<User> AddUserToFacultyAsync(AssignUserDto assignUserDto);
    Task<Faculty> RemoveUserFromFacultyAsync(AssignUserDto assignUserDto);
    Task<Faculty> FacultyExistsAsinc(string name);
    Task<bool> IsUserAddedToFacultyAsync(AssignUserDto assignUserDto);
    
}