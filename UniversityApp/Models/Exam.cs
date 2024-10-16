using UniversityManagament.Models;

namespace UniversityApp.Models
{
    public class Exam
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }    

        public Guid ExamPeriodId { get; set; }
        public ExamPeriod ExamPeriod { get; set; }

        public List<UserExam>? UserExams { get; set; } = new List<UserExam>();
    }
}
