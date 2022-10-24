namespace ProjectTracker.Data.Entities
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LeadId { get; set; }
        public Employee Lead { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
