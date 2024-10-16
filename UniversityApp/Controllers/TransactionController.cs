using Microsoft.AspNetCore.Mvc;
using UniversityApp.Models;
using UniversityApp.Services.Interfaces;
using UniversityManagament.Models;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IFinanceService _financeService;
    private readonly ILogger<TransactionController> _logger;

    public TransactionController(ITransactionService transactionService, IFinanceService financeService, ILogger<TransactionController> logger)
    {
        _transactionService = transactionService;
        _financeService = financeService;
        _logger = logger;
    }

    //[Authorize(Policy = "FinancePolicy")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BankTransaction>>> GetTransactions()
    {
        try
        {
            return await _transactionService.GetTransactions();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching universities.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        } 
    }

    //[Authorize(Policy = "FinancePolicy")]
    [HttpGet("{id}")]
    public async Task<ActionResult<BankTransaction>> GetTransaction(Guid id)
    {
        try
        {
            var transaction = await _transactionService.DeleteTransaction(id);
            if (transaction == null)
                return NotFound($"Transaction with id : {id} not found");

            return transaction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching universities.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        } 
    }

    [HttpPost]
    public async Task<ActionResult<BankTransaction>> PostTransaction(BankTransactionDto transaction)
    {
        try
        {
            if (await _financeService.GetFinanceByIdAsync(transaction.FinanceId) == null)
                return NotFound($"Finance with id : {transaction.FinanceId} not found");

            var transactionToAdd = await _transactionService.PostTransaction(transaction);

            return Ok(transactionToAdd);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching universities.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        } 
    }


    //[Authorize(Policy = "FinancePolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        try
        {
            var transaction = await _transactionService.DeleteTransaction(id);
            if (transaction == null)
                return NotFound($"Transaction with id : {id} not found");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching universities.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        } 
    }
}
