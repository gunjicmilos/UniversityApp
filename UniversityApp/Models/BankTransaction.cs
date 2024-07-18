using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityManagament.Models;

public class BankTransaction
{
    [Key]
    public Guid Id { get; set; }
    public string Description { get; set; }
    public Guid FinanceId { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
}

public enum TransactionType
{
    Income,
    Expense
}