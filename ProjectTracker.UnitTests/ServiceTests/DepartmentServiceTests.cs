using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Moq;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.Services;
using ProjectTracker.Core.ViewModels.Department;
using ProjectTracker.Infrastructure.Data.Common;
using ProjectTracker.Infrastructure.Data.Entities;
using ProjectTracker.Infrastructure.DataConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.UnitTests.ServiceTests
{
    [TestFixture]
    public class DepartmentServiceTests : UnitTestsBase
    {
        private IRepository repo;
        private IDepartmentService departmentService;

        [OneTimeSetUp]
        public void Setup()
        {
            var mockRepo = new Mock<IRepository>();

            repo = new Repository(data);
            departmentService = new DepartmentService(repo);
        }

        [Test]
        public async Task GetCountAsync_ReturnsCorrectCount()
        {
            var expected = 2;

            var actual = await departmentService.GetCountAsync();

            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public async Task CreateAsync_CreatesSuccessfully()
        {
            var oldCount = await departmentService.GetCountAsync();

            await departmentService.CreateAsync(new CreateDepartmentViewModel()
            {
                Name = "test",
                LeadId = "421365c1-f8d2-4a4b-abc7-aaabaa82117d",
            });

            var afterAdding = await departmentService.GetCountAsync();

            Assert.That(afterAdding, Is.EqualTo(oldCount + 1));
        }

        [Test]
        public void CreateAsync_ThrowsNullReferenceExzception_IfLeadDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await departmentService.CreateAsync(new CreateDepartmentViewModel()
                {
                    Name = "test",
                    LeadId = "non-existent-id"
                });
            });
        }

        [Test]
        public void CreateAsync_ThrowsArgumentExzception_IfLeadIsAlreadyLead()
        {
            var data = repo.All<Employee>();

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await departmentService.CreateAsync(new CreateDepartmentViewModel()
                {
                    Name = "test",
                    LeadId = "6da2e2f3-c43b-4a68-beea-4da7915b1528"
                });
            });
        }

        [Test]
        public async Task DeleteAsync_RemovesDepartment()
        {
            var startCount = await departmentService.GetCountAsync();

            var users = repo.All<Employee>().ToList();

            await departmentService.DeleteAsync(new Guid("94874ad5-4e3a-4bea-bac8-c7a3002eb205"));

            var afterDelete = await departmentService.GetCountAsync();

            Assert.IsTrue(afterDelete < startCount);
        }


        [Test]
        public void DeleteAsync_ThrowsNullReferenceException_IfDepartmentDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(()
                => departmentService.DeleteAsync(Guid.NewGuid()));
        }

        [Test]
        public async Task EditAsync_EditsDepartment()
        {
            var departmentId = "aabcdf93-5aa9-46dc-bbf3-854d551a8b6d";
            var newName = Guid.NewGuid().ToString();

            await departmentService.EditAsync(new EditDepartmentViewModel()
            {
                Id = new Guid(departmentId),
                Name = newName,
                LeadId = "6da2e2f3-c43b-4a68-beea-4da7915b1528"
            });

            var department = await departmentService.GetDepartmentDetailsAsync(new Guid(departmentId));

            Assert.IsTrue(department.Name == newName);
        }

        [Test]
        public void EditAsync_ThrowsNullReferenceException_WhenDepartmentDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(()
                => departmentService.EditAsync(new EditDepartmentViewModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "asdasdasda",
                    LeadId = "6da2e2f3-c43b-4a68-beea-4da7915b1528"
                }));
        }

        [Test]
        public void GetAllAsync_ReturnsAllDepartments()
        {
            var expected = data.Departments.Where(d => d.IsActive).Count();

            var actual = departmentService.GetAllAsync().Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetAllIdAndNameAsync_ReturnsAllDepartments()
        {
            var expected = data.Departments.Where(d => d.IsActive).Count();

            var actual = departmentService.GetAllIdAndNameAsync().Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetDepartmentDetailsAsync_ReturnsCorrect()
        {
            var expected = await data.Departments.FirstAsync();

            var actual = await departmentService.GetDepartmentDetailsAsync(expected.Id);

            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
        }

        [Test]
        public async Task GetEditDetailsAsync_ReturnsCorrect()
        {
            var expected = await data.Departments.FirstAsync();

            var actual = await departmentService.GetEditDetailsAsync(expected.Id);

            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
        }

        [Test]
        public async Task GetInactiveDepartmentsAsync_ReturnsCorrect()
        {
            var expected = await data.Departments.Where(d => !d.IsActive).CountAsync();

            var actual = departmentService.GetInactiveDepartmentsAsync().Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetPossibleLeadersAsync_ReturnsCorrect()
        {
            var expected = await data.Employees.Where(e => e.IsActive && e.LeadedDepartmentId == null)
                .CountAsync();

            var actual = departmentService.GetPossibleLeadersAsync().Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
