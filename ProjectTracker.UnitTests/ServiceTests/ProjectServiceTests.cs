using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Internal;
using ProjectTracker.Core.Services;
using ProjectTracker.Core.ViewModels.Project;
using ProjectTracker.Infrastructure.Data.Common;
using ProjectTracker.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.UnitTests.ServiceTests
{
    public class ProjectServiceTests : UnitTestsBase
    {
        private IRepository repo;
        private ProjectService projectService;

        [SetUp]
        public async Task Setup()
        {
            repo = new Repository(data);
            projectService = new ProjectService(repo);

            await SetAllToActive();
        }

        [Test]
        public async Task CreateAsync_CreatesProjectAsync()
        {
            var startCount = await data.Projects
                .Where(p => p.IsActive)
                .CountAsync();

            await projectService.CreateAsync(new CreateProjectViewModel()
            {
                Name = "test test",
                Description = "test test test test test",
                DepartmentId = new Guid("94874ad5-4e3a-4bea-bac8-c7a3002eb205")
            });

            var countAfterCreate = await data.Projects
                .Where(p => p.IsActive)
                .CountAsync();

            Assert.That(countAfterCreate, Is.EqualTo(startCount + 1));
        }

        [Test]
        public async Task CreateAsync_ThrowsNullReferenceException_WhenDepartmentDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(async ()
                => await projectService.CreateAsync(new CreateProjectViewModel()
                {
                    DepartmentId = Guid.NewGuid()
                }));
        }

        [Test]
        public async Task DeleteAsync_ThrowsNullReferenceException_WhenProjectDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(async ()
                => await projectService.DeleteAsync(Guid.NewGuid()));
        }

        [Test]
        public async Task DeleteAsync_SetsProjectToInactive()
        {
            var beforeDelete = await data.Projects
                .Where(p => p.IsActive).CountAsync();

            await projectService.DeleteAsync(new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8"));

            var afterDelete = await data.Projects
                .Where(p => p.IsActive).CountAsync();

            Assert.That(afterDelete, Is.EqualTo(beforeDelete - 1));
        }

        [Test]
        public async Task DeleteAsync_SetsEmployeeProjectsToInactive()
        {
            await projectService.DeleteAsync(new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8"));

            var project = await data.Projects
                .Where(p => p.Id == new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8"))
                .Include(p => p.AssignedEmployees)
                .FirstAsync();

            foreach (var ep in project.AssignedEmployees)
            {
                Assert.That(!ep.IsActive);
            }
        }

        [Test]
        public async Task DeleteAsync_SetsProjectsTicketsToInactive()
        {
            var projecttt = data.Projects
                .Where(p => p.Id == new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8"))
                .First();

            await projectService.DeleteAsync(new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8"));

            var project = await data.Projects
                .Where(p => p.Id == new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8"))
                .Include(p => p.Tickets)
                .FirstAsync();

            foreach (var ticket in project.Tickets)
            {
                Assert.That(!ticket.IsActive);
            }
        }

        [Test]
        public async Task EditProjectAsync_ThrowsNullReferenceException_WhenProjectDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(async ()
                => await projectService.EditProjectAsync(new EditProjectViewModel()
                {
                    Id = Guid.NewGuid()
                }));
        }

        [Test]
        public async Task EditProjectAsync_EditsProjectCorrect()
        {
            var project = await data.Projects
                .Where(p => p.Id == new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8"))
                .FirstAsync();

            string name = project.Name;
            string description = project.Description;

            string newName = "edited";
            string newDescription = "edited descriptiopn";

            await projectService.EditProjectAsync(new EditProjectViewModel()
            {
                Id = new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8"),
                Name = newName,
                Description = newDescription
            });

            Assert.That(name != project.Name);
            Assert.That(description != project.Description);
            Assert.That(project.Name == newName);
            Assert.That(project.Description == newDescription);
        }

        [Test]
        public async Task EditProjectAsync_ReturnsProjectId()
        {
            var projectId = new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8");

            var returnValue = await projectService.EditProjectAsync(new EditProjectViewModel()
            {
                Id = projectId,
                Name = "SomeName",
                Description = "test test test test test"
            });

            Assert.That(returnValue, Is.EqualTo(projectId));
        }

        [Test]
        public async Task GetAllProjectsAsync_ReturnsCorrect()
        {
            var expected = await data.Projects
                .Where(p => p.IsActive)
                .CountAsync();

            var actual = projectService.GetAllProjectsAsync()
                .Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetCountAsync_ReturnsCorrect()
        {
            var expected = await data.Projects
                .Where(p => p.IsActive)
                .CountAsync();

            var actual = await projectService.GetCountAsync();
        }

        [Test]
        public async Task GetEditDetailsAsync_ReturnsCorrect()
        {
            var project = await data.Projects
                .Where(p => p.IsActive && p.Id == new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8"))
                .FirstOrDefaultAsync();

            var expectedName = project.Name;
            var expectedDescription = project.Description;

            var actualProject = await projectService
                .GetEditDetailsAsync(new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8"));

            var actualName = actualProject.Name;
            var actualDescription = actualProject.Description;

            Assert.That(actualName, Is.EqualTo(expectedName));
            Assert.That(actualDescription, Is.EqualTo(expectedDescription));
        }

        public void GetEditDetailsAsync_ThrowsNullReferenceException_WhenProjectDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(async ()
                => await projectService.GetEditDetailsAsync(Guid.NewGuid()));
        }


        [Test]
        public async Task GetAllIdsAndNamesAsync_ReturnsCorrect()
        {
            var expected = await data.Projects
                .Where(p => p.IsActive)
                .CountAsync();

            var actual = projectService.GetIdsAndNamesAsync()
                .Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetInactiveProjectsAsync_ReturnsCorrect()
        {
            var expected = await data.Projects
                .Where(p => !p.IsActive)
                .CountAsync();

            var actual = projectService.GetInactiveProjectsAsync()
                .Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetProjectDetailsByIdAsync_ReturnsCorrect()
        {
            var project = await data.Projects
                .Where(p => p.IsActive && p.Id == new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8"))
                .FirstOrDefaultAsync();

            var expectedName = project.Name;
            var expectedDescription = project.Description;

            var actualProject = await projectService
                .GetProjectDetailsByIdAsync(new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8"));

            var actualName = actualProject.Name;
            var actualDescription = actualProject.Description;

            Assert.That(actualName, Is.EqualTo(expectedName));
            Assert.That(actualDescription, Is.EqualTo(expectedDescription));
        }

        [Test]
        public async Task GetProjectDetailsByIdAsync_ThrowsNullReferenceException_WhenProjectDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(async ()
                => await projectService.GetProjectDetailsByIdAsync(Guid.NewGuid()));
        }

        private async Task SetAllToActive()
        {
            await data.Projects.ForEachAsync(
                p => p.IsActive = true);

            await data.SaveChangesAsync();
        }
    }
}
