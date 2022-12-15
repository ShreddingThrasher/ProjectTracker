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
        /// <summary>
        /// Gets the Count of all active projects.
        /// </summary>
        /// <returns>Count as int</returns>
        Task<int> GetCountAsync();


        /// <summary>
        /// Gets all active Projects
        /// </summary>
        /// <returns>All active projects as ProjectViewModel</returns>
        Task<IEnumerable<ProjectViewModel>> GetAllProjectsAsync();


        /// <summary>
        /// Gets all innactive Projects
        /// </summary>
        /// <returns>All innactive projects as ProjectViewModel</returns>
        Task<IEnumerable<ProjectViewModel>> GetInactiveProjectsAsync();


        /// <summary>
        /// Creates new Project
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        Task CreateAsync(CreateProjectViewModel model);


        /// <summary>
        /// Gets details for an active Project
        /// </summary>
        /// <param name="id">ProjectId</param>
        /// <returns>Project details</returns>
        Task<ProjectDetailsViewModel> GetProjectDetailsByIdAsync(Guid id);


        /// <summary>
        /// Gets Id and Name for all active Projects
        /// </summary>
        /// <returns>All projects Id and Name</returns>
        Task<IEnumerable<ProjectIdNameViewModel>> GetIdsAndNamesAsync();


        /// <summary>
        /// Gets details for a Project to be edited
        /// </summary>
        /// <param name="id">ProjectId</param>
        /// <returns>Project details</returns>
        Task<EditProjectViewModel> GetEditDetailsAsync(Guid id);


        /// <summary>
        /// Edits Project
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>ProjectId</returns>
        Task<Guid> EditProjectAsync(EditProjectViewModel model);


        /// <summary>
        /// Deletes Project
        /// </summary>
        /// <param name="id">ProjectId</param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);


        /// <summary>
        /// Get all Projects that the user is assigned to.
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns>Collection of ProjectViewModel</returns>
        Task<IEnumerable<ProjectViewModel>> UserProjectsAsync(string userId);
    }
}
