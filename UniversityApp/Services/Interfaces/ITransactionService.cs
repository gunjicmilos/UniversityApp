using Microsoft.AspNetCore.Mvc;
using UniversityApp.Models;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services.Interfaces;

public interface ITransactionService
{
    Task<List<BankTransaction>> GetTransactions();
    Task<BankTransaction> GetTransaction(Guid id);
    Task<BankTransaction> PostTransaction(BankTransactionDto transaction);
    Task<BankTransaction> DeleteTransaction(Guid id);
}