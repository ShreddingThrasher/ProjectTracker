using ProjectTracker.Core.Constants;
using ProjectTracker.Core.ViewModels.Project;
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
    public class EditTicketViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(TicketConstants.TitleMaxLength,
            MinimumLength = TicketConstants.TitleMinLength)]
        [RegularExpression(ValidationRegex.PropertyRegex,
            ErrorMessage = "Contains unallowed characters")]
        public string Title { get; set; }

        [Required]
        [EnumDataType(typeof(Priority), ErrorMessage = "Incorrect value!")]
        public Priority Priority { get; set; }

        public string[] Priorities { get; set; }
            = new string[] { "Low", "Medium", "High" };

        [Required]
        [EnumDataType(typeof(Status), ErrorMessage = "Incorrect value!")]
        public Status Status { get; set; }

        public string[] Statuses { get; set; }
            = new string[] { "Open", "InProgress", "Done" };
    }
}
