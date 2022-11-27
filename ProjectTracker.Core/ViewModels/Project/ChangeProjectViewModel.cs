using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Project
{
    public class ChangeProjectViewModel
    {
        [Required]
        [Display(Name = "Project")]
        public Guid ProjectId { get; set; }

        public IEnumerable<ProjectIdNameViewModel> Projects { get; set; }
    }
}
