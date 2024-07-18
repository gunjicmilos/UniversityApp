namespace UniversityManagament.Models.Dto
{
    public class ExamPeriodDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndData { get; set; }
        public Guid FacultyId { get; set; }
    }
}
