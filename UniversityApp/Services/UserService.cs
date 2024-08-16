using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Services;

public class UserService : IUserService
{
    private readonly DataContext _context;
    public UserService(DataContext context)
    {
        _context = context;
    }
        public async Task<ActionResult<IEnumerable<UserDtoWithFaculties>>> GetUsersAsync([FromQuery] string? name = null, [FromQuery] string? email = null, [FromQuery] Guid? facultyId = null)
        {
            var users = await _context.Users
                .Include(u => u.UserFaculties)
                .ThenInclude(uf => uf.Faculty)
                .Select(u => new UserDtoWithFaculties
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    Faculties = u.UserFaculties.Select(uf => new FacultyDto
                    {
                        Id = uf.Faculty.Id,
                        Name = uf.Faculty.Name,
                        UniversityId = uf.Faculty.UniversityId,
                        Location = uf.Faculty.Location
                    }).ToList()
                })
                .ToListAsync();

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

        public async Task<ActionResult<UserDto>> GetUserAsync(Guid id)
        {
            var user = await _context.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role
                })
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<ActionResult<UserDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new User()
            {
                Name = createUserDto.Name,
                Password = createUserDto.Password,
                Email = createUserDto.Email,
                Role = createUserDto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
            return userDto;
        }

        public async Task<ActionResult<User>> UpdateUserAsync(Guid id, CreateUserDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(id);
            user.Name = updateUserDto.Name;
            user.Email = updateUserDto.Email;
            user.Password = updateUserDto.Password;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<ActionResult<User>> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
      
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }
        
        public Task<User> ValidateUserAsync(string username, string password)
        {
            var user =  _context.Users.FirstOrDefaultAsync(u => u.Name == username && u.Password == password);
            return user;
        }
}