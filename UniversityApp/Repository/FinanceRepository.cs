using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using UniversityApp.Data;
using UniversityApp.Models;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository;

[Authorize]
public class FinanceRepository : IFinanceRepository
{
    private readonly DataContext _context;

    public FinanceRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<FinanceReadDto>> GetAllFinancesAsync()
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

    public async Task<Finance> GetFinanceAsync(Guid id)
    {
        return await _context.Finances.FindAsync(id);
    }
    
    public async Task CreateFinance(Finance finance)
    {
        _context.Finances.Add(finance);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFinance(Finance finance)
    {
        _context.Finances.Update(finance);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFinance(Finance finance)
    {
        _context.Finances.Remove(finance);
        await _context.SaveChangesAsync();    }
}