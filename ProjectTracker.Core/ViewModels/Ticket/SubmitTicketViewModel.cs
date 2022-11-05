using ProjectTracker.Infrastructure.Data.Entities.Enums;
using ProjectTracker.Infrastructure.DataConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Ticket
{
    public  class SubmitTicketViewModel
    {
        public SubmitTicketViewModel()
        {
            Priorities = new List<string>
            {
                "Low",
                "Medium",
                "High"
            };
        }

        [Required]
        [StringLength(TicketConstants.TitleMaxLength,
            MinimumLength = TicketConstants.TitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(TicketConstants.DescriptionMaxLength,
            MinimumLength = TicketConstants.DescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public Priority Priority { get; set; }

        public ICollection<string> Priorities { get; set; }
    }
}
