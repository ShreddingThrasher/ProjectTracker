using ProjectTracker.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTracker.Data.Entities
{
    public class Department
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(DepartmentConstants.NameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string LeadId { get; set; }

        [ForeignKey(nameof(LeadId))]
        public Employee Lead { get; set; }

        [InverseProperty("Department")]
        public ICollection<Employee> Employees { get; set; }

        public ICollection<Project> Projects { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
