using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services.Interfaces;

public interface ISubjectService
{
    Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjects(string? name = null, Guid? departmentId = null);
    Task<ActionResult<SubjectDto>> GetSubjectsById(Guid id);
    Task<ActionResult<SubjectDto>> CreateSubject(CreateSubjectDto createSubjectDto);
    Task<ActionResult<Subject>> UpdateSubject(Guid id, CreateSubjectDto updateSubjectDto);
    Task<ActionResult<Subject>> DeleteSubject(Guid id);
}