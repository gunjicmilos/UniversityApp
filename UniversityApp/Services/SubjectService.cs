using Microsoft.AspNetCore.Mvc;
using UniversityApp.Models;
using UniversityApp.Repository.IRepository;
using UniversityApp.Services.Interfaces;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Services;

public class SubjectService : ISubjectService
{
    private readonly ISubjectRepository _subjectRepository;
    private readonly IUserRepository _userRepository;

    public SubjectService(ISubjectRepository subjectRepository, IUserRepository userRepository)
    {
        _subjectRepository = subjectRepository;
        _userRepository = userRepository;
    }
    
    public async Task<IEnumerable<SubjectDto>> GetSubjects([FromQuery] string? name = null, [FromQuery] Guid? departmentId = null)
    {
        var subjects = await _subjectRepository.GetAllSubjectsAsync();

            if (!string.IsNullOrWhiteSpace(name))
            {
                subjects = subjects.Where(u => u.Name.Contains(name)).ToList();
            }

            if (departmentId != null)
            {
                subjects = subjects.Where(u => u.DepartmentId == departmentId).ToList();
            }

            return subjects;
        }

        public async Task<SubjectDto> GetSubjectsById(Guid id)
        {
            var subjects = await _subjectRepository.GetSubjectAsync(id);

            return subjects;
        }

        public async Task<SubjectDto> CreateSubject(CreateSubjectDto createSubjectDto)
        {
            var subject = new Subject
            {
                Name = createSubjectDto.Name,
                DepartmentId = createSubjectDto.DepartmentId,
                Espb = createSubjectDto.Espb
            };

            if (createSubjectDto.UsersIds != null && createSubjectDto.UsersIds.Any())
            {
                var users = await _userRepository.GetUsersByUserIds(createSubjectDto);
                subject.UserSubjects.AddRange(users.Select(u => new UserSubject { UserId = u.Id, SubjectId = subject.Id }));
            }

            await _subjectRepository.CreateSubject(subject);

            var subjectDto = new SubjectDto
            {
                Id = subject.Id,
                Name = subject.Name,
                DepartmentId = subject.DepartmentId,
                Espb = subject.Espb,
                UserIds = subject.UserSubjects.Select(us => us.UserId).ToList()
            };

            return subjectDto;
        }

        public async Task<Subject> UpdateSubject(Guid id, CreateSubjectDto updateSubjectDto)
        {
            var subject = await _subjectRepository.GetSubjectFromDbAsync(id);

            subject.Name = updateSubjectDto.Name; ;
            subject.DepartmentId = updateSubjectDto.DepartmentId;
            subject.Espb = updateSubjectDto.Espb;

            subject.UserSubjects.Clear();
            if (updateSubjectDto.UsersIds != null && updateSubjectDto.UsersIds.Any())
            {
                var users = await _userRepository.GetUsersByUserIds(updateSubjectDto);
                subject.UserSubjects.AddRange(users.Select(u => new UserSubject { UserId = u.Id, SubjectId = subject.Id }));
            }

            await _subjectRepository.UpdateSubject(subject); 

            return subject;
        }

        public async Task<Subject> DeleteSubject(Guid id)
        {
            var subject = await _subjectRepository.GetSubjectFromDbAsync(id);

            await _subjectRepository.DeleteSubject(subject);

            return subject;
        }

        public async Task AddUserToSubject(UserSubject userSubject)
        {
            await _subjectRepository.AddUserToSubject(userSubject);
        }

        public async Task<IEnumerable<UserSubject>> GetSubjects(Guid id)
        {
            return await _subjectRepository.GetAllSubjectsOfUserAsync(id);
        }
}