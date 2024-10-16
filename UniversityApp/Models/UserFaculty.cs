using UniversityManagament.Models;

namespace UniversityApp.Models
{
    public class UserFaculty
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid FacultyId { get; set; }
        public Faculty Faculty { get; set; }
    }
}
