using System.ComponentModel.DataAnnotations;

namespace UniversityManagament.Models.Dto;

public class FinanceCreateDto
{
    [Required]
    public Guid FacultyId { get; set; }

    [Required]
    [Range(0.01, 1000000)]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(255)]
    public string Description { get; set; }

    [Required]
    public DateTime Date { get; set; }
}