using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services;

public class SubjectService
{
    private readonly DataContext _context;

    public SubjectService(DataContext context)
    {
        _context = context;
    }
    
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjects([FromQuery] string? name = null, [FromQuery] Guid? departmentId = null)
        {
            var subjects = await _context.Subjects
                .Include(s => s.UserSubjects)
                .Select(s => new SubjectDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    DepartmentId = s.DepartmentId,
                    UserIds = s.UserSubjects.Select(us => us.UserId).ToList()
                }).ToListAsync();

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

        public async Task<ActionResult<SubjectDto>> GetSubjectsById(Guid id)
        {
            var subjects = await _context.Subjects
                .Include(s => s.UserSubjects)
                .Select(s => new SubjectDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    DepartmentId = s.DepartmentId,
                    UserIds = s.UserSubjects.Select(us => us.UserId).ToList()
                })
                .FirstOrDefaultAsync(s => s.Id == id);

            return subjects;
        }

        public async Task<ActionResult<SubjectDto>> CreateSubject(CreateSubjectDto createSubjectDto)
        {
            var subject = new Subject
            {
                Name = createSubjectDto.Name,
                DepartmentId = createSubjectDto.DepartmentId,
            };

            if (createSubjectDto.UsersIds != null && createSubjectDto.UsersIds.Any())
            {
                var users = await _context.Users.Where(u => createSubjectDto.UsersIds.Contains(u.Id)).ToListAsync();
                subject.UserSubjects.AddRange(users.Select(u => new UserSubject { UserId = u.Id, SubjectId = subject.Id }));
            }

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            var subjectDto = new SubjectDto
            {
                Id = subject.Id,
                Name = subject.Name,
                DepartmentId = subject.DepartmentId,
                UserIds = subject.UserSubjects.Select(us => us.UserId).ToList()
            };

            return subjectDto;
        }

        public async Task<ActionResult<Subject>> UpdateSubject(Guid id, CreateSubjectDto updateSubjectDto)
        {
            var subject = await _context.Subjects.Include(s => s.UserSubjects).FirstOrDefaultAsync(s => s.Id == id);

            subject.Name = updateSubjectDto.Name; ;
            subject.DepartmentId = updateSubjectDto.DepartmentId;

            subject.UserSubjects.Clear();
            if (updateSubjectDto.UsersIds != null && updateSubjectDto.UsersIds.Any())
            {
                var users = await _context.Users.Where(u => updateSubjectDto.UsersIds.Contains(u.Id)).ToListAsync();
                subject.UserSubjects.AddRange(users.Select(u => new UserSubject { UserId = u.Id, SubjectId = subject.Id }));
            }
            
            _context.Entry(subject).State = EntityState.Modified;
            await _context.SaveChangesAsync();  

            return subject;
        }

        public async Task<ActionResult<Subject>> DeleteSubject(Guid id)
        {
            var subject = await _context.Subjects.Include(s => s.UserSubjects).FirstOrDefaultAsync(s => s.Id == id);

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return subject;
        }
}