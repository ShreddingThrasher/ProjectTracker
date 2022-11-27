using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Project
{
    public class DeleteProjectViewModel
    {
        public DeleteProjectViewModel()
        {
            Projects = new List<ProjectIdNameViewModel>();
        }

        public Guid Id { get; set; }

        public IEnumerable<ProjectIdNameViewModel> Projects { get; set; }
    }
}
