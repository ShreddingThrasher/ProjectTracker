namespace ProjectTracker.Data.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<EmployeeProject> AssignedEmployees { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
