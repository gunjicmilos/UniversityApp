namespace UniversityManagament.Models.Dto
{
    public class SubjectDto
    {
        public Guid Id { get; set; }    
        public string Name { get; set; }
        public Guid DepartmentId { get; set; }
        public int Espb { get; set; }
        public List<Guid> UserIds { get; set; }
    }
}
