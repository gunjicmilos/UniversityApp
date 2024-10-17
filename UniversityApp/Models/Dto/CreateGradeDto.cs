namespace UniversityManagament.Models.Dto;

public class CreateGradeDto
{
    public Guid SubjectId { get; set; } 
    public Guid UserId { get; set; } 
    public int Grade { get; set; }
}