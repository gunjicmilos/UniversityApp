using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository.IRepository;

public interface IUserRepository
{
    Task<List<UserDtoWithFaculties>> GetUsersAsync();
    Task<List<User>> GetUsersByUserIds(CreateSubjectDto subjectDto);
    Task<List<User>> GetUsersByUserIds(CreateExamDto subjectDto);
    Task<UserDto> GetUserAsync(Guid id);
    Task<User> GetUserFromDbAsync(Guid id);
    Task CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(User user);
    Task<User> ValidateUserAsync(string username, string password);
}