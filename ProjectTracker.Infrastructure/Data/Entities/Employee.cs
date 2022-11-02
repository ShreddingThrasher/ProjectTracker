using Microsoft.AspNetCore.Identity;
using ProjectTracker.Infrastructure.DataConstants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTracker.Infrastructure.Data.Entities
{
    public class Employee : IdentityUser
    {
        [Required]
        [MaxLength(EmployeeConstants.FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(EmployeeConstants.LastNameMaxLength)]
        public string LastName { get; set; }

        public Guid? DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }

        public Guid? LeadedDepartmentId { get; set; }

        [ForeignKey(nameof(LeadedDepartmentId))]
        public Department LeadedDepartment { get; set; }

        public ICollection<EmployeeProject> EmployeesProjects { get; set; }

        [InverseProperty("Submitter")]
        public ICollection<Ticket> SubmittedTickets { get; set; }

        [InverseProperty("AssignedEmployee")]
        public ICollection<Ticket> AssignedTickets { get; set; }

        public ICollection<TicketComment> Comments { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
