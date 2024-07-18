namespace UniversityManagament.Models.Dto
{
    public class ExamDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public Guid SubjectId { get; set; }

        public Guid ExamPeriodId { get; set; }

        public List<Guid> UserIds { get; set; } = new List<Guid>();
    }
}
