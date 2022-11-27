using ProjectTracker.Core.ViewModels.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Contracts
{
    public interface IProjectService
    {
        Task<int> GetCount();

        Task<IEnumerable<ProjectViewModel>> GetAllProjects();

        Task Create(CreateProjectViewModel model);

        Task<ProjectDetailsViewModel> GetProjectDetailsById(Guid id);

        Task<IEnumerable<ProjectIdNameViewModel>> GetIdsAndNamesAsync();

        Task<EditProjectViewModel> GetEditDetailsAsync(Guid id);

        Task<Guid> EditProjectAsync(EditProjectViewModel model);

        Task DeleteAsync(Guid id);
    }
}
