namespace UniversityManagament.Models.Dto
{
    public class CreateSubjectDto
    {
        public string Name { get; set; }
        public Guid DepartmentId { get; set; }
        public int Espb { get; set; }
        public List<Guid> UsersIds { get; set; }
    }
}
