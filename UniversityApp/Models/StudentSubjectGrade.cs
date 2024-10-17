namespace UniversityApp.Models;

public class StudentSubjectGrade
{
    public Guid Id { get; set; } 
    public Guid SubjectId { get; set; } 
    public Guid UserId { get; set; } 
    public int Grade { get; set; }
}