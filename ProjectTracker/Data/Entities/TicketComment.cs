namespace ProjectTracker.Data.Entities
{
    public class TicketComment
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string CommenterId { get; set; }
        public Employee Commenter { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
