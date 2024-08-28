using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Repository.IRepository;

public interface IFinanceRepository
{
    Task<List<FinanceReadDto>> GetAllFinancesAsync();
    Task<Finance> GetFinanceAsync(Guid id);
    Task CreateFinance(Finance finance);
    Task UpdateFinance(Finance finance);
    Task DeleteFinance(Finance finance);
}