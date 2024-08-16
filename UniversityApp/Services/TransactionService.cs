using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Services;

public class TransactionService : ITransactionService
{
    private readonly DataContext _context; 

    public TransactionService(DataContext context)
    {
        _context = context;
    }

    public async Task<ActionResult<IEnumerable<BankTransaction>>> GetTransactions()
    {
        return await _context.BankTransactions.ToListAsync();
    }
    
    public async Task<ActionResult<BankTransaction>> GetTransaction(int id)
    {
        var transaction = await _context.BankTransactions.FindAsync(id);
        return transaction;
    }
    
    public async Task<ActionResult<BankTransaction>> PostTransaction(BankTransactionDto transaction)
    {
        var transactionToAdd = new BankTransaction()
        {
            Description = transaction.Description,
            FinanceId = transaction.FinanceId,
            Amount = transaction.Amount,
            Date = transaction.Date,
            Type = transaction.Type
        };
        
        
        
        _context.BankTransactions.Add(transactionToAdd);
        await _context.SaveChangesAsync();

        // Update Finance amount based on transaction type
        if (transaction.Type == TransactionType.Income)
        {
            var finance = await _context.Finances.FindAsync(transaction.FinanceId);
            if (finance != null)
            {
                finance.Amount += transaction.Amount;
                await _context.SaveChangesAsync();
            }
        }
        else if (transaction.Type == TransactionType.Expense)
        {
            var finance = await _context.Finances.FindAsync(transaction.FinanceId);
            if (finance != null)
            {
                finance.Amount -= transaction.Amount;
                await _context.SaveChangesAsync();
            }
        }

        return transactionToAdd;
    }


    public async Task<BankTransaction> DeleteTransaction(Guid id)
    {
        var transaction = await _context.BankTransactions.FindAsync(id);
 
        _context.BankTransactions.Remove(transaction);
        await _context.SaveChangesAsync();

        return transaction;
    }

    private bool TransactionExists(int id)
    {
        var transaction = _context.BankTransactions.FindAsync(id);
        if (transaction != null)
        {
            return true;
        }
        return false;
    }
}