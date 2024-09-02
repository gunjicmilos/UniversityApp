using Microsoft.EntityFrameworkCore;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<UserDtoWithFaculties>> GetUsersAsync()
    {
        return await _context.Users
            .Include(u => u.UserFaculties)
            .ThenInclude(uf => uf.Faculty)
            .Select(u => new UserDtoWithFaculties
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                Index = u.Index,
                Faculties = u.UserFaculties.Select(uf => new FacultyDto
                {
                    Id = uf.Faculty.Id,
                    Name = uf.Faculty.Name,
                    UniversityId = uf.Faculty.UniversityId,
                    Location = uf.Faculty.Location
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<List<User>> GetUsersByUserIds(CreateSubjectDto subjectDto)
    {
        return await _context.Users.Where(u => subjectDto.UsersIds.Contains(u.Id)).ToListAsync();
    }
    
    public async Task<List<User>> GetUsersByUserIds(CreateExamDto subjectDto)
    {
        return await _context.Users.Where(u => subjectDto.UserIds.Contains(u.Id)).ToListAsync();
    }

    public async Task<UserDto> GetUserAsync(Guid id)
    {
        return await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                Index = u.Index,
            })
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetUserFromDbAsync(Guid id)
    {
        return await _context.Users
            .Select(u => new User
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                Index = u.Index,
            })
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> ValidateUserAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == username && u.Password == password);
        return user;
    }
}