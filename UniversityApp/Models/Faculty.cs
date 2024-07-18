namespace UniversityManagament.Models
{
    public class Faculty
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Location { get; set; }
        public Guid UniversityId { get; set; }
        public University? University { get; set; }
        public ICollection<UserFaculty>? UserFaculties { get; set; } 
        public ICollection<Department> Departments { get; set; }

        public ICollection<ExamPeriod> ExamPeriods { get; set;}
    }
}
