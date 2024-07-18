using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityManagament.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly DataContext _context; 

    public TransactionController(DataContext context)
    {
        _context = context;
    }

    // GET: api/transaction
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BankTransaction>>> GetTransactions()
    {
        return await _context.BankTransactions.ToListAsync();
    }

    // GET: api/transaction/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BankTransaction>> GetTransaction(int id)
    {
        var transaction = await _context.BankTransactions.FindAsync(id);

        if (transaction == null)
        {
            return NotFound();
        }

        return transaction;
    }

    [HttpPost]
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

        return Ok(transactionToAdd);
    }


    // DELETE: api/transaction/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        var transaction = await _context.BankTransactions.FindAsync(id);
        if (transaction == null)
        {
            return NotFound();
        }

        _context.BankTransactions.Remove(transaction);
        await _context.SaveChangesAsync();

        return NoContent();
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
