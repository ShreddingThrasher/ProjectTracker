namespace ProjectTracker.Data.Entities
{
    public class EmployeeProject
    {
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
