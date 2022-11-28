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
        Task<int> GetCountAsync();

        Task<IEnumerable<ProjectViewModel>> GetAllProjectsAsync();

        Task<IEnumerable<ProjectViewModel>> GetInactiveProjectsAsync();

        Task CreateAsync(CreateProjectViewModel model);

        Task<ProjectDetailsViewModel> GetProjectDetailsByIdAsync(Guid id);

        Task<IEnumerable<ProjectIdNameViewModel>> GetIdsAndNamesAsync();

        Task<EditProjectViewModel> GetEditDetailsAsync(Guid id);

        Task<Guid> EditProjectAsync(EditProjectViewModel model);

        Task DeleteAsync(Guid id);
    }
}
