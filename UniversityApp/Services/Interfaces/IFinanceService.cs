using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services.Interfaces;

public interface IFinanceService
{
    Task<IEnumerable<FinanceReadDto>> GetAllFinancesAsync();
    Task<FinanceReadDto> GetFinanceByIdAsync(Guid id);
    Task<FinanceReadDto> CreateFinanceAsync(FinanceCreateDto financeCreateDto);
    Task<bool> UpdateFinanceAsync(Guid id, FinanceCreateDto financeUpdateDto);
    Task<bool> DeleteFinanceAsync(Guid id);
}