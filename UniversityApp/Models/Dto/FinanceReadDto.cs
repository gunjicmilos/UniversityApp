namespace UniversityManagament.Models.Dto;

public class FinanceReadDto
{
    public Guid Id { get; set; }
    public Guid FacultyId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
}