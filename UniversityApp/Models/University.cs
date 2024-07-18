namespace UniversityManagament.Models
{
    public class University
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Location { get; set; }
        public ICollection<Faculty> Faculties { get; set; }
    }
}
