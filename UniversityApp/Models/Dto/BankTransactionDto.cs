using System.ComponentModel.DataAnnotations.Schema;
using UniversityApp.Models;

namespace UniversityManagament.Models.Dto;

public class BankTransactionDto
{
    public string Description { get; set; }
    public Guid FinanceId { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public string? Payer { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
}