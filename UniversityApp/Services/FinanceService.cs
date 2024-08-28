using Microsoft.EntityFrameworkCore;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Services;

public class FinanceService : IFinanceService
{
    private readonly IFinanceRepository _financeRepository;

        public FinanceService(IFinanceRepository financeRepository)
        {
            _financeRepository = financeRepository;
        }

        public async Task<List<FinanceReadDto>> GetAllFinancesAsync()
        {
            return await _financeRepository.GetAllFinancesAsync();
        }

        public async Task<FinanceReadDto> GetFinanceByIdAsync(Guid id)
        {
            var finance = await _financeRepository.GetFinanceAsync(id);
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

            await _financeRepository.CreateFinance(finance);

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
            var finance = await _financeRepository.GetFinanceAsync(id);
            if (finance == null) return false;

            finance.FacultyId = financeUpdateDto.FacultyId;
            finance.Amount = financeUpdateDto.Amount;
            finance.Description = financeUpdateDto.Description;
            finance.Date = financeUpdateDto.Date;

            await _financeRepository.UpdateFinance(finance);

            return true;
        }

        public async Task<bool> DeleteFinanceAsync(Guid id)
        {
            var finance = await _financeRepository.GetFinanceAsync(id);
            if (finance == null) return false;

            await _financeRepository.DeleteFinance(finance);

            return true;
        }
}