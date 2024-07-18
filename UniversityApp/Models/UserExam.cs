namespace UniversityManagament.Models
{
    public class UserExam
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ExamId { get; set; }
        public Exam Exam { get; set; }
    }
}
