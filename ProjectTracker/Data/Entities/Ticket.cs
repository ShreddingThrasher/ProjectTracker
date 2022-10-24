using Humanizer;
using ProjectTracker.Data.Entities;
using ProjectTracker.Data.Entities.Enums;
using System;

namespace ProjectTracker.Data.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IssuerId { get; set; }
        public Employee Issuer { get; set; }
        public DateTime CreatedOn { get; set; }
        public Priority Priority { get; set; }
        public string? AssignedEmployeeId { get; set; }
        public Employee? AssignedEmployee { get; set; }
        public Status Status { get; set; }
        public ICollection<TicketChange> History { get; set; }
    }
}
