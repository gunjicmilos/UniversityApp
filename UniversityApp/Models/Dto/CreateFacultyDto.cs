namespace UniversityManagament.Models.Dto
{
    public class CreateFacultyDto
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public Guid UniversityId { get; set; }
    }
}
