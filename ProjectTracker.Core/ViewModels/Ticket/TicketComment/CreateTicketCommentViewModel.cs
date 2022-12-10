using ProjectTracker.Core.Constants;
using ProjectTracker.Infrastructure.DataConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Ticket.TicketComment
{
    public class CreateTicketCommentViewModel
    {
        [Required]
        public Guid TicketId { get; set; }

        [Required]
        [StringLength(CommentConstants.MessageMaxLength,
            MinimumLength = CommentConstants.MessageMinLength)]
        [RegularExpression(ValidationRegex.DescriptionAndMessageRegex,
            ErrorMessage = "Contains unallowed characters")]
        public string Message { get; set; }
    }
}
