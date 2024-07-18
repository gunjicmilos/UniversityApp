namespace UniversityManagament.Models.Dto
{
    public class FacultyDto
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public string Location { get; set; }
        public Guid UniversityId { get; set; }
        //public List<Guid> UserIds { get; set; }
        public ICollection<DepartmentDto> Departments { get; set; }

    }
}
