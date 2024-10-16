using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniversityManagament.Models;

namespace UniversityApp.Models;

public class Finance
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid FacultyId { get; set; }

    [ForeignKey("FacultyId")]
    public Faculty Faculty { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public DateTime Date { get; set; }
}