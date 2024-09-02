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
    private readonly IFinanceService _financeService;

    public TransactionController(ITransactionService transactionService, IFinanceService financeService)
    {
        _transactionService = transactionService;
        _financeService = financeService;
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
            return NotFound($"Transaction with id : {id} not found");
        }

        return transaction;
    }

    [HttpPost]
    public async Task<ActionResult<BankTransaction>> PostTransaction(BankTransactionDto transaction)
    {
        if (await _financeService.GetFinanceByIdAsync(transaction.FinanceId) == null)
            return NotFound($"Finance with id : {transaction.FinanceId} not found");
        
        var transactionToAdd = await _transactionService.PostTransaction(transaction);

        return Ok(transactionToAdd);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        var transaction = await _transactionService.DeleteTransaction(id);
        if (transaction == null)
        {
            return NotFound($"Transaction with id : {id} not found");
        }

        return NoContent();
    }
}
