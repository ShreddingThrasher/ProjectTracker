using Moq;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.Services;
using ProjectTracker.Core.ViewModels.Department;
using ProjectTracker.Infrastructure.Data.Common;
using ProjectTracker.Infrastructure.Data.Entities;
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

            repo = mockRepo.Object;

            repo = new Repository(data);
            departmentService = new DepartmentService(repo);
        }

        [Test]
        public async Task GetCountAsync_ReturnsCorrectCount()
        {
            var expected = 1;

            var actual = await departmentService.GetCountAsync();

            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public async Task GetCountAsync_ReturnsCorrectCountAfterAdding()
        {
            var oldCount = await departmentService.GetCountAsync();

            await departmentService.CreateAsync(new CreateDepartmentViewModel()
            {
                Name = "test",
                LeadId = "421365c1-f8d2-4a4b-abc7-aaabaa82117d"
            });

            var afterAdding = await departmentService.GetCountAsync();

            Assert.That(afterAdding, Is.EqualTo(oldCount + 1));
        }

    }
}
