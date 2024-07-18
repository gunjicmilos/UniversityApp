using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityManagament.Models.Dto;

public class BankTransactionDto
{
    public string Description { get; set; }
    public Guid FinanceId { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
}