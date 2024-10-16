using UniversityApp.Models;
using UniversityManagament.Models;

namespace UniversityApp.Repository.IRepository;

public interface ITransactionRepository
{
    Task<List<BankTransaction>> GetTransactionsAsync();
    Task<BankTransaction> GetTransaction(Guid id);
    Task CreateTransaction(BankTransaction transaction);
    Task DeleteTransaction(BankTransaction transaction);
}