namespace UniversityManagament.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public ICollection<UserFaculty> UserFaculties { get; set; }
        public ICollection<UserSubject> UserSubjects { get; set; }

        public ICollection<UserExam> UserExams { get; set; }
    }
}
