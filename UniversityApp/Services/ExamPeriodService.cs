using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services;

public class ExamPeriodService
{
    private readonly DataContext _context;

    public ExamPeriodService(DataContext context)
    {
        _context = context;
    }

    public async Task<ActionResult<IEnumerable<ExamPeriodDto>>> GetExamPeriods()
        {
            var examPeriods = await _context.ExamPeriods
                .Select(ep => new ExamPeriodDto
                {
                    Id = ep.Id,
                    Name = ep.Name,
                    StartDate = ep.StartDate,
                    EndData = ep.EndDate,
                    FacultyId = ep.FacultyId,
                }).ToListAsync();

            return examPeriods;
        }

        public async Task<ActionResult<ExamPeriodDto>> GetExamPeriod(Guid id)
        {
            var examPeriod = await _context.ExamPeriods
                .Select(ep => new ExamPeriodDto
                {
                    Id = ep.Id,
                    Name = ep.Name,
                    StartDate = ep.StartDate,
                    EndData = ep.EndDate,
                    FacultyId = ep.FacultyId,
                }).FirstOrDefaultAsync(e => e.Id == id);

            return examPeriod;
        }

        public async Task<ActionResult<ExamPeriodDto>> CreateExamPeriod(CreateExamPeriodDto createExamPeriodDto)
        {
            var examPeriod = new ExamPeriod
            {
                Name = createExamPeriodDto.Name,
                StartDate = createExamPeriodDto.StartData,
                EndDate = createExamPeriodDto.EndDate,
                FacultyId = createExamPeriodDto.FacultyId
            };

            _context.ExamPeriods.Add(examPeriod);
            await _context.SaveChangesAsync();

            var examPeriodDto = new ExamPeriodDto
            {
                Id = examPeriod.Id,
                Name = examPeriod.Name,
                StartDate = examPeriod.StartDate,
                EndData = examPeriod.EndDate,
                FacultyId = examPeriod.FacultyId,
            };

            return examPeriodDto;
        }

        public async Task<ActionResult<ExamPeriod>> UpdateExamPerion(Guid id, CreateExamPeriodDto updateExamPeriodDto)
        {
            var examPeriod = await _context.ExamPeriods.FindAsync(id);
            
            examPeriod.Name = updateExamPeriodDto.Name;
            examPeriod.StartDate = updateExamPeriodDto.StartData;
            examPeriod.EndDate = updateExamPeriodDto.EndDate;
            examPeriod.FacultyId = updateExamPeriodDto.FacultyId;

            _context.Entry(examPeriod).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return examPeriod;
        }

        public async Task<ActionResult<ExamPeriod>> DeleteExamPerion(Guid id)
        {
            var examPeriod = await _context.ExamPeriods.FindAsync(id);

            _context.ExamPeriods.Remove(examPeriod);
            await _context.SaveChangesAsync();

            return examPeriod;
        }
}