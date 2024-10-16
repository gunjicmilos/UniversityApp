using UniversityManagament.Models;

namespace UniversityApp.Models
{
    public class Subject
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public int Espb { get; set; }
        public Guid DepartmentId { get; set; }
        
        public Department Department { get; set; }
        public Guid ProfessorId { get; set; }
        public List<UserSubject> UserSubjects { get; set; } = new List<UserSubject>();
    }
}
