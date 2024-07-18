namespace UniversityManagament.Models.Dto
{
    public class CreateExamPeriodDto
    {
        public string Name { get; set; }
        public DateTime StartData { get; set; }
        public DateTime EndDate { get; set; }
        public Guid FacultyId { get; set; }
    }
}
