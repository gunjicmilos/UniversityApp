using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Services.Interfaces;

public interface ITransactionService
{
    Task<ActionResult<IEnumerable<BankTransaction>>> GetTransactions();
    Task<ActionResult<BankTransaction>> GetTransaction(int id);
    Task<ActionResult<BankTransaction>> PostTransaction(BankTransactionDto transaction);
    Task<BankTransaction> DeleteTransaction(Guid id);
}