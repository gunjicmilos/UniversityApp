using UniversityApp.Models;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityApp.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IFinanceRepository _financeRepository;

    public TransactionService(ITransactionRepository transactionRepository, IFinanceRepository financeRepository)
    {
        _transactionRepository = transactionRepository;
        _financeRepository = financeRepository;
    }

    public async Task<List<BankTransaction>> GetTransactions()
    {
        return await _transactionRepository.GetTransactionsAsync();
    }
    
    public async Task<BankTransaction> GetTransaction(Guid id)
    {
        var transaction = await _transactionRepository.GetTransaction(id);
        return transaction;
    }
    
    public async Task<BankTransaction> PostTransaction(BankTransactionDto transaction)
    {
        var transactionToAdd = new BankTransaction()
        {
            Description = transaction.Description,
            FinanceId = transaction.FinanceId,
            Amount = transaction.Amount,
            Date = transaction.Date,
            Type = transaction.Type
        };

        await _transactionRepository.CreateTransaction(transactionToAdd);

        if (transaction.Type == TransactionType.Income)
        {
            var finance = await _financeRepository.GetFinanceAsync(transaction.FinanceId);
            if (finance != null)
            {
                finance.Amount += transaction.Amount;
                await _financeRepository.UpdateFinance(finance);
            }
        }
        else if (transaction.Type == TransactionType.Expense)
        {
            var finance = await _financeRepository.GetFinanceAsync(transaction.FinanceId);
            if (finance != null)
            {
                finance.Amount -= transaction.Amount;
                await _financeRepository.UpdateFinance(finance);
            }
        }

        return transactionToAdd;
    }


    public async Task<BankTransaction> DeleteTransaction(Guid id)
    {
        var transaction = await _transactionRepository.GetTransaction(id);
        if (transaction == null)
            return null;

        await _transactionRepository.DeleteTransaction(transaction);

        return transaction;
    }

    private bool TransactionExists(Guid id)
    {
        var transaction = _transactionRepository.GetTransaction(id);
        if (transaction != null)
        {
            return true;
        }
        return false;
    }
}