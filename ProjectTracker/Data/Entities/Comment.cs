using ProjectTracker.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTracker.Data.Entities
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(CommentConstants.MessageMaxLength)]
        public string Message { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string CommenterId { get; set; }

        [ForeignKey(nameof(CommenterId))]
        public Employee Commenter { get; set; }

        [Required]
        public Guid TicketId { get; set; }

        [ForeignKey(nameof(TicketId))]
        public Ticket Ticket { get; set; }
    }
}
