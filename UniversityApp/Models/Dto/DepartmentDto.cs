namespace UniversityManagament.Models.Dto
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid FacultyId { get; set; }
    }
}
