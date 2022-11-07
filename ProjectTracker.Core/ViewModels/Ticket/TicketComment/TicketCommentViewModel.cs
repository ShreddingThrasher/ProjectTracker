using ProjectTracker.Infrastructure.DataConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Ticket.TicketComment
{
    public class TicketCommentViewModel
    {
        public string Message { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CommenterId { get; set; } = null!;

        public string Commenter { get; set; } = null!;
    }
}
