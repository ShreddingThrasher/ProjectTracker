using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTracker.Data.Entities
{
    public class Change
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Property { get; set; }

        [Required]
        public string OldValue { get; set; }

        [Required]
        public string NewValue { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public Guid TicketId { get; set; }

        [ForeignKey(nameof(TicketId))]
        public Ticket Ticket { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
