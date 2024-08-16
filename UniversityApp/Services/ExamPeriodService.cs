using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Services;

public class ExamPeriodService : IExamPeriodService
{
    private readonly DataContext _context;
    private readonly IExamPeriodRepository _periodRepository;

    public ExamPeriodService(DataContext context, IExamPeriodRepository periodRepository)
    {
        _context = context;
        _periodRepository = periodRepository;
    }

    public async Task<IEnumerable<ExamPeriodDto>> GetExamPeriodsAsync()
    {
        return await _periodRepository.GetAllExamPeriodsAsync();
    }

    public async Task<ExamPeriodDto> GetExamPeriodByIdAsync(Guid id)
    {
        return await _periodRepository.GetExamPeriodByIdAsync(id);
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

            await _periodRepository.AddExamPeriodAsync(examPeriod);

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
    
        public async Task<ActionResult<ExamPeriod>> UpdateExamPeriod(Guid id, CreateExamPeriodDto updateExamPeriodDto)
        {
            var examPeriod = await _context.ExamPeriods.FindAsync(id);
            
            examPeriod.Name = updateExamPeriodDto.Name;
            examPeriod.StartDate = updateExamPeriodDto.StartData;
            examPeriod.EndDate = updateExamPeriodDto.EndDate;
            examPeriod.FacultyId = updateExamPeriodDto.FacultyId;

            await _periodRepository.UpdateExamPeriodAsync(examPeriod);

            return examPeriod;
        }

        public async Task<ActionResult<ExamPeriod>> DeleteExamPeriod(Guid id)
        {
            var examPeriod = await _context.ExamPeriods.FindAsync(id);

            await _periodRepository.DeleteExamPeriodAsync(examPeriod);

            return examPeriod;
        }
}