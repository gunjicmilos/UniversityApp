namespace UniversityManagament.Models
{
    public class ExamPeriod
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Guid FacultyId { get; set; }
        public Faculty Faculty { get; set; }

    }
}
