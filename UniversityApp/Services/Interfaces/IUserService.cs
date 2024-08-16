using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services.Interfaces;

public interface IUserService
{
    Task<ActionResult<IEnumerable<UserDtoWithFaculties>>> GetUsersAsync(string? name = null, string? email = null, Guid? facultyId = null);
    Task<ActionResult<UserDto>> GetUserAsync(Guid id);
    Task<ActionResult<UserDto>> CreateUserAsync(CreateUserDto createUserDto);
    Task<ActionResult<User>> UpdateUserAsync(Guid id, CreateUserDto updateUserDto);
    Task<ActionResult<User>> DeleteUserAsync(Guid id);
    Task<User> ValidateUserAsync(string username, string password);
}