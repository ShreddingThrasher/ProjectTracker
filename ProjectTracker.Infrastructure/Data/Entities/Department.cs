using ProjectTracker.Infrastructure.DataConstants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTracker.Infrastructure.Data.Entities
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
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public ICollection<Project> Projects { get; set; } = new List<Project>();

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        [Required]
        public bool IsActive { get; set; }
    }
}
