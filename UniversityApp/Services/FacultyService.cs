using UniversityApp.Models;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityApp.Services;

public class FacultyService : IFacultyService
{
    private readonly IFacultyRepository _facultyRepository;

    public FacultyService(IFacultyRepository facultyRepository)
    {
        _facultyRepository = facultyRepository;
    }

        public async Task<IEnumerable<FacultyDto>> GetFaculties(string? name = null, string? location = null, Guid? UniversityId = null)
        {
            var faculties = await _facultyRepository.GetAllFacultiesAsync();

            if (!string.IsNullOrWhiteSpace(name))
            {
                faculties = faculties.Where(f => f.Name.Contains(name)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                faculties = faculties.Where(f => f.Location.Contains(location)).ToList();
            }

            if (UniversityId != null)
            {
                faculties = faculties.Where(f => f.UniversityId == UniversityId).ToList();
            }

            return faculties;
        }

        public async Task<FacultyDto> GetFaculty(Guid id)
        {
            return await _facultyRepository.GetFacultyByIdAsync(id);
        }

        public async Task<FacultyWithoutDepartmentsAndUsersDto> CreateFaculty(CreateFacultyDto createFacultyDto)
        {
            var faculty = new Faculty()
            {
                Name = createFacultyDto.Name,
                Location = createFacultyDto.Location,
                UniversityId = createFacultyDto.UniversityId,
            };

            await _facultyRepository.AddFacultyAsync(faculty);

            var facultyDto = new FacultyWithoutDepartmentsAndUsersDto()
            {
                Id = faculty.Id,
                Name = faculty.Name,
                Location = faculty.Location,
            };

            return facultyDto;
        }

        public async Task<Faculty> UpdateFaculty(Guid id, CreateFacultyDto updateFacultyDto)
        {
            var faculty = await _facultyRepository.GetFacultyByIdFromDbAsync(id);

            if (faculty == null)
            {
                return null;
            }
            
            faculty.Name = updateFacultyDto.Name;
            faculty.Location = updateFacultyDto.Location;
            faculty.UniversityId = updateFacultyDto.UniversityId;

            faculty.UserFaculties.Clear();

            await _facultyRepository.UpdateFacultyAsync(faculty);

            return faculty;
        }

        

        public async Task<Faculty> DeleteFaculty(Guid id)
        {
            var faculty = await _facultyRepository.GetFacultyByIdFromDbAsync(id);
            
            if (faculty == null)
            {
                return null;
            }

            await _facultyRepository.DeleteFacultyAsync(id);

            return faculty;
        }

        public async Task<User> AddUserToFaculty(AssignUserDto addUserDto)
        {
             return await _facultyRepository.AddUserToFacultyAsync(addUserDto);
        }

        public async Task<Faculty> DeleteUserFromFaculty(AssignUserDto removeUserDto)
        {
            return await _facultyRepository.RemoveUserFromFacultyAsync(removeUserDto);
        }

        public async Task<bool> FacultyExists(string name)
        {
            var faculty = await _facultyRepository.FacultyExistsAsinc(name);
            if (faculty != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsUserAddedToFaculty(AssignUserDto assignUserDto)
        {
            return await _facultyRepository.IsUserAddedToFacultyAsync(assignUserDto);
        }
}