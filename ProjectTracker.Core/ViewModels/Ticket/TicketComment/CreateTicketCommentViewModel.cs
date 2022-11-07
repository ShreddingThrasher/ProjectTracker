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
        [StringLength(CommentConstants.MessageMaxLength,
            MinimumLength = CommentConstants.MessageMinLength)]
        public string Message { get; set; }
    }
}
