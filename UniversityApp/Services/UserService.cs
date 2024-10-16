using Microsoft.AspNetCore.Mvc;
using UniversityApp.Models;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityApp.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
        public async Task<List<UserDtoWithFaculties>> GetUsersAsync([FromQuery] string? name = null, [FromQuery] string? email = null, [FromQuery] Guid? facultyId = null)
        {
            var users = await _userRepository.GetUsersAsync();

            if (!string.IsNullOrWhiteSpace(name))
            {
                users = users.Where(u => u.Name.Contains(name)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                users = users.Where(u => u.Email.Contains(email)).ToList();
            }

            if (facultyId != null)
            {
                users = users.Where(u => u.Faculties.Any(uf => uf.Id == facultyId)).ToList();
            }
            
            return users;
        }

        public async Task<UserDto> GetUserAsync(Guid id)
        {
            var user = await _userRepository.GetUserAsync(id);
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new User()
            {
                Name = createUserDto.Name,
                Password = createUserDto.Password,
                Email = createUserDto.Email,
                Role = createUserDto.Role
            };

            await _userRepository.CreateUserAsync(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
            return userDto;
        }

        public async Task<User> UpdateUserAsync(Guid id, CreateUserDto updateUserDto)
        {
            var user = await _userRepository.GetUserFromDbAsync(id);

            if (user == null)
            {
                return null;
            }
            user.Name = updateUserDto.Name;
            user.Email = updateUserDto.Email;
            user.Password = updateUserDto.Password;

            await _userRepository.UpdateUserAsync(user);

            return user;
        }

        public async Task<User> DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetUserFromDbAsync(id);
            
            if (user == null)
            {
                return null;
            }

            await _userRepository.DeleteUserAsync(user);

            return user;
        }
        
        public async Task<User> ValidateUserAsync(string username, string password)
        {
            return await _userRepository.ValidateUserAsync(username, password);
        }
}