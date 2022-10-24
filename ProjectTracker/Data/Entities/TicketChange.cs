namespace ProjectTracker.Data.Entities
{
    public class TicketChange
    {
        public Guid Id { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime Date { get; set; }
        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
