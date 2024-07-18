namespace UniversityManagament.Models
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid FacultyId { get; set; }
        public Faculty Faculty { get; set; }
        public List<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
