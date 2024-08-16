using Microsoft.EntityFrameworkCore;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Services;

public class FinanceService : IFinanceService
{
    private readonly DataContext _context;

        public FinanceService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FinanceReadDto>> GetAllFinancesAsync()
        {
            return await _context.Finances
                .Select(f => new FinanceReadDto
                {
                    Id = f.Id,
                    FacultyId = f.FacultyId,
                    Amount = f.Amount,
                    Description = f.Description,
                    Date = f.Date
                }).ToListAsync();
        }

        public async Task<FinanceReadDto> GetFinanceByIdAsync(Guid id)
        {
            var finance = await _context.Finances.FindAsync(id);
            if (finance == null) return null;

            return new FinanceReadDto
            {
                Id = finance.Id,
                FacultyId = finance.FacultyId,
                Amount = finance.Amount,
                Description = finance.Description,
                Date = finance.Date
            };
        }

        public async Task<FinanceReadDto> CreateFinanceAsync(FinanceCreateDto financeCreateDto)
        {
            var finance = new Finance
            {
                Id = Guid.NewGuid(),
                FacultyId = financeCreateDto.FacultyId,
                Amount = financeCreateDto.Amount,
                Description = financeCreateDto.Description,
                Date = financeCreateDto.Date
            };

            _context.Finances.Add(finance);
            await _context.SaveChangesAsync();

            return new FinanceReadDto
            {
                Id = finance.Id,
                FacultyId = finance.FacultyId,
                Amount = finance.Amount,
                Description = finance.Description,
                Date = finance.Date
            };
        }

        public async Task<bool> UpdateFinanceAsync(Guid id, FinanceCreateDto financeUpdateDto)
        {
            var finance = await _context.Finances.FindAsync(id);
            if (finance == null) return false;

            finance.FacultyId = financeUpdateDto.FacultyId;
            finance.Amount = financeUpdateDto.Amount;
            finance.Description = financeUpdateDto.Description;
            finance.Date = financeUpdateDto.Date;

            _context.Finances.Update(finance);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteFinanceAsync(Guid id)
        {
            var finance = await _context.Finances.FindAsync(id);
            if (finance == null) return false;

            _context.Finances.Remove(finance);
            await _context.SaveChangesAsync();

            return true;
        }
}