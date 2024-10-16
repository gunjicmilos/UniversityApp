using UniversityApp.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Services.Interfaces;

public interface IUserService
{
    Task<List<UserDtoWithFaculties>> GetUsersAsync(string? name = null, string? email = null, Guid? facultyId = null);
    Task<UserDto> GetUserAsync(Guid id);
    Task<User> GetUserByEmailAsync(string email);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<User> UpdateUserAsync(Guid id, CreateUserDto updateUserDto);
    Task<User> DeleteUserAsync(Guid id);
    Task<User> ValidateUserAsync(string username, string password);
}