    using Microsoft.EntityFrameworkCore;
    using UniversityApp.Repository.IRepository;
    using UniversityManagament.Data;
    using UniversityManagament.Models;
    using UniversityManagament.Models.Dto;

    namespace UniversityApp.Repository;

    public class FacultyRepository : IFacultyRepository
    {
        private readonly DataContext _context;

        public FacultyRepository(DataContext context)
        {
            _context = context;
        }
        
        public async Task<List<FacultyDto>> GetAllFacultiesAsync()
        {
            var faculties = await _context.Faculties
                .Include(f => f.UserFaculties) // Ako vam je potrebno da uključite UserFaculties, možete dodati i ovu liniju
                .Include(f => f.Departments) // Uključivanje povezanih Departments
                .Select(f => new FacultyDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Location = f.Location,
                    UniversityId = f.UniversityId,
                    Departments = f.Departments.Select(d => new DepartmentDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        FacultyId = d.FacultyId,
                    }).ToList(),
                })
                .ToListAsync();

            return faculties;
        }
        
        public async Task<FacultyDto> GetFacultyByIdAsync(Guid id)
        {
            var faculties = await _context.Faculties
                .Include(f => f.UserFaculties)
                .Select(f => new FacultyDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Location = f.Location,
                    UniversityId = f.UniversityId,
                    Departments = f.Departments.Select(d => new DepartmentDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        FacultyId = d.FacultyId,
                    }).ToList(),
                })
                .FirstOrDefaultAsync(f => f.Id == id);

            return faculties;
        }
        
        public async Task<Faculty> GetFacultyByIdFromDbAsync(Guid id)
        {
            var faculties = await _context.Faculties
                .Include(f => f.UserFaculties)
                .FirstOrDefaultAsync(f => f.Id == id);

            return faculties;
        }
        
        public async Task AddFacultyAsync(Faculty faculty)
        {
            _context.Faculties.Add(faculty);
            await _context.SaveChangesAsync();
        }
        
        public async Task UpdateFacultyAsync(Faculty faculty)
        {
            _context.Entry(faculty).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteFacultyAsync(Guid id)
        {
            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty != null)
            {
                _context.Faculties.Remove(faculty);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<User> AddUserToFacultyAsync(AssignUserDto addUserDto)
        {
            var faculty = await _context.Faculties
                .Include(faculty => faculty.UserFaculties)
                .FirstOrDefaultAsync(f => f.Id == addUserDto.FacultyId);

            var user = await _context.Users
                .Include(u => u.UserFaculties)
                .FirstOrDefaultAsync(u => u.Id == addUserDto.UserId);
            if (faculty == null || user == null)
            {
                return null;
            }

            var userFaculty = new UserFaculty()
            {
                UserId = addUserDto.UserId,
                FacultyId = addUserDto.FacultyId,
            };

            faculty.UserFaculties.Add(userFaculty);

            await _context.SaveChangesAsync();
            return user;
        }
        
        public async Task<Faculty> RemoveUserFromFacultyAsync(AssignUserDto removeUserDto)
        {
            var faculty = await _context.Faculties
                .Include(faculty => faculty.UserFaculties)
                .FirstOrDefaultAsync(f => f.Id == removeUserDto.FacultyId);
                
            var userFaculty = faculty.UserFaculties
                .FirstOrDefault(uf => uf.UserId == removeUserDto.UserId);

            faculty.UserFaculties.Remove(userFaculty);
            await _context.SaveChangesAsync();

            return faculty;
        }

        public Task<Faculty> FacultyExistsAsinc(string name)
        {
            return _context.Faculties.FirstOrDefaultAsync(f => f.Name == name);
        }

        public async Task<bool> IsUserAddedToFacultyAsync(AssignUserDto assignUserDto)
        {
            var result = await _context.UserFaculties.Where
                (uf => uf.UserId == assignUserDto.UserId && uf.FacultyId == assignUserDto.FacultyId).FirstOrDefaultAsync();
            if (result == null)
                return false;
            return true;
        }
    }