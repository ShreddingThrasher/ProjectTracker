using ProjectTracker.Infrastructure.Data.Entities.Enums;
using ProjectTracker.Infrastructure.DataConstants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTracker.Infrastructure.Data.Entities
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(TicketConstants.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(TicketConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }

        [Required]
        public string SubmitterId { get; set; }

        [ForeignKey(nameof(SubmitterId))]
        public Employee Submitter { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public Guid ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }

        public string? AssignedEmployeeId { get; set; }

        [ForeignKey(nameof(AssignedEmployeeId))]
        public Employee? AssignedEmployee { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public Priority Priority { get; set; }

        public ICollection<TicketComment> Comments { get; set; }

        public ICollection<TicketChange> History { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
