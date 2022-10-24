using Microsoft.AspNetCore.Identity;

namespace ProjectTracker.Data.Entities
{
    public class Employee : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<EmployeeProject> EmployeesProjects { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
