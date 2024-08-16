using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService; 

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    // GET: api/transaction
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BankTransaction>>> GetTransactions()
    {
        return await _transactionService.GetTransactions();
    }

    // GET: api/transaction/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BankTransaction>> GetTransaction(Guid id)
    {
        var transaction = await _transactionService.DeleteTransaction(id);

        if (transaction == null)
        {
            return NotFound();
        }

        return transaction;
    }

    [HttpPost]
    public async Task<ActionResult<BankTransaction>> PostTransaction(BankTransactionDto transaction)
    {
        var transactionToAdd = await _transactionService.PostTransaction(transaction);

        return Ok(transactionToAdd);
    }


    // DELETE: api/transaction/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        var transaction = await _transactionService.DeleteTransaction(id);
        if (transaction == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}
