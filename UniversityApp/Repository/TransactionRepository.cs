using Microsoft.EntityFrameworkCore;
using UniversityApp.Data;
using UniversityApp.Models;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Models;

namespace UniversityApp.Repository;

public class TransactionRepository : ITransactionRepository
{
    private readonly DataContext _context;

    public TransactionRepository(DataContext context)
    {
        _context = context;
    }
    
    public async Task<List<BankTransaction>> GetTransactionsAsync()
    {
        return await _context.BankTransactions.ToListAsync();
    }

    public async Task<BankTransaction> GetTransaction(Guid id)
    {
        return await _context.BankTransactions.FindAsync(id);
    }

    public async Task CreateTransaction(BankTransaction transaction)
    {
        _context.BankTransactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTransaction(BankTransaction transaction)
    {
        _context.BankTransactions.Remove(transaction);
        await _context.SaveChangesAsync();
    }
}