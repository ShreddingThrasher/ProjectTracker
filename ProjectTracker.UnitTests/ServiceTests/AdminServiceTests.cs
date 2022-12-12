using Microsoft.AspNetCore.Identity;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.Services;
using ProjectTracker.Infrastructure.Data.Common;
using ProjectTracker.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace ProjectTracker.UnitTests.ServiceTests
{
    public class AdminServiceTests : UnitTestsBase
    {
        private IRepository repo;
        private IAdminService adminService;
        private Mock<RoleManager<IdentityRole>> roleManagerMock;
        private Mock<UserManager<Employee>> userManagerMock;

        [SetUp]
        public async Task Setup()
        {
            var userStoreMock = new Mock<IUserStore<Employee>>();
            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();

            userManagerMock = new Mock<UserManager<Employee>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
            roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                roleStoreMock.Object, null, null, null, null);

            var employee = data.Employees
                    .Where(e => e.IsActive && e.Id == "cd77e188-a292-42ed-b165-015b9f1d8c51")
                    .FirstOrDefaultAsync();

            

            repo = new Repository(data);
            adminService = new AdminService(
                roleManagerMock.Object, repo, userManagerMock.Object);

            //User Manager mock setup
            userManagerMock.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                .Returns(data.Employees
                    .Where(e => e.IsActive && e.Id == "6da2e2f3-c43b-4a68-beea-4da7915b1528")
                    .FirstAsync());

            userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<Employee>(), It.IsAny<string>()).Result)
                .Returns(IdentityResult.Success);


            //Role Manager Mock Setup
            roleManagerMock.Setup(rm => rm.Roles)
                .Returns(new List<IdentityRole>()
                {
                    new IdentityRole
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    }
                }.AsQueryable());

        }

        [Test]
        public async Task AddToRoleAsync_ThrowsNullReferenceException_IfEmployeeDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(async ()
                => await adminService.AddToRoleAsync("any", "any"));
        }

        [Test]
        public async Task AddToRoleAsync_ThrowsNullReferenceException_IfRoleDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(async ()
                => await adminService.AddToRoleAsync("6da2e2f3-c43b-4a68-beea-4da7915b1528", "any"));
        }

        [Test]
        public async Task AddToRoleAsync_WorksCorrect()
        {
            var result = await adminService.AddToRoleAsync(
                "6da2e2f3-c43b-4a68-beea-4da7915b1528", "Admin");

            Assert.That(result.Succeeded);
        }

        [Test]
        public async Task AssignToDepartmentAsync_ThrowsNullReferenceException_IfEmployeeDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(async ()
                => await adminService.AssignToDepartmentAsync("non existent", Guid.NewGuid()));
        }

        [Test]
        public async Task AssignToDepartmentAsync_ThrowsNullReferenceException_IfDepartmentDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(async ()
                => await adminService.AssignToDepartmentAsync("6da2e2f3-c43b-4a68-beea-4da7915b1528", Guid.NewGuid()));
        }
    }
}
