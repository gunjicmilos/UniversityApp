namespace UniversityManagament.Models.Dto
{
    public class UserDtoWithFaculties
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string? Index { get; set; }
        public ICollection<FacultyDto> Faculties { get; set;}
    }
}
