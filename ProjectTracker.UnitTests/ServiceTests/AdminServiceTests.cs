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
using ProjectTracker.Infrastructure.DataConstants;
using ProjectTracker.Core.ViewModels.Admin;

namespace ProjectTracker.UnitTests.ServiceTests
{
    public class AdminServiceTests : UnitTestsBase
    {
        private IRepository repo;
        private IAdminService adminService;
        private Mock<RoleManager<IdentityRole>> roleManagerMock;
        private Mock<UserManager<Employee>> userManagerMock;

        private List<IdentityRole> roles;

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

            roles = new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            };

            roleManagerMock.Setup(rm => rm.Roles)
                .Returns(roles.AsQueryable());

            roleManagerMock.Setup(rm => rm.CreateAsync(It.IsAny<IdentityRole>()))
                .Callback((IdentityRole role) => roles.Add(role));

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

        [Test]
        public async Task AssignToDepartmentAsync_ThrowsArgumentException_IfEmployeeIsAlreadyInThatDepartment()
        {
            Assert.ThrowsAsync<ArgumentException>(async ()
                => await adminService.AssignToDepartmentAsync(
                    "6da2e2f3-c43b-4a68-beea-4da7915b1528", new Guid("aabcdf93-5aa9-46dc-bbf3-854d551a8b6d")));
        }

        [Test]
        public async Task AssignToDepartmentAsync_ThrowsArgumentException_IfEmployeeIsAlreadyLeaderOfADepartment()
        {
            Assert.ThrowsAsync<ArgumentException>(async ()
                => await adminService.AssignToDepartmentAsync(
                    "6da2e2f3-c43b-4a68-beea-4da7915b1528", new Guid("aabcdf93-5aa9-46dc-bbf3-854d551a8b6d")));
        }

        [Test]
        public async Task AssignToDepartmentAsync_AssignsCorrect()
        {
            var employee = await data.Employees
                .Where(e => e.IsActive && e.Id == "cd77e188-a292-42ed-b165-015b9f1d8c51")
                .FirstAsync();

            var departmentId = new Guid("aabcdf93-5aa9-46dc-bbf3-854d551a8b6d");

            var startDepartment = employee.DepartmentId;

            await adminService.AssignToDepartmentAsync(employee.Id, departmentId);

            Assert.That(startDepartment, Is.Not.EqualTo(departmentId));
            Assert.That(employee.DepartmentId, Is.EqualTo(departmentId));
        }

        [Test]
        public async Task AssignToProjectAsync_ThrowsNullReferenceException_WhenEmployeeDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(async ()
                => await adminService.AssignToProjectAsync("non existent", Guid.NewGuid()));
        }

        [Test]
        public async Task AssignToProjectAsync_ThrowsNullReferenceException_WhenProjectDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(async ()
                => await adminService.AssignToProjectAsync("cd77e188-a292-42ed-b165-015b9f1d8c51", Guid.NewGuid()));
        }

        [Test]
        public async Task AssignToProjectAsync_ThrowsArgumentException_WhenEmployeeIsAlreadyInThatProject()
        {
            var projects = await data.Projects
                .Where(p => p.IsActive)
                .ToListAsync();

            Assert.ThrowsAsync<ArgumentException>(async ()
                => await adminService.AssignToProjectAsync(
                    "6da2e2f3-c43b-4a68-beea-4da7915b1528", new Guid("bae3e81b-bfe0-418a-8082-4672bf1f98cd")));
        }

        [Test]
        public async Task AssignToProjectAsync_AssignsCorrect()
        {
            var epBeforeAssign = await data.EmployeesProjects
                .Where(p => p.IsActive)
                .CountAsync();

            await adminService.AssignToProjectAsync(
                "421365c1-f8d2-4a4b-abc7-aaabaa82117d", new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8"));

            var epAfterAssign = await data.EmployeesProjects
                .Where(p => p.IsActive)
                .CountAsync();

            var ep = await data.EmployeesProjects
                .Where(ep => ep.EmployeeId == "421365c1-f8d2-4a4b-abc7-aaabaa82117d"
                    && ep.ProjectId == new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8"))
                .FirstOrDefaultAsync();

            Assert.That(epAfterAssign, Is.EqualTo(epBeforeAssign + 1));
            Assert.That(ep, Is.Not.EqualTo(null));
        }

        [Test]
        public async Task CreateRoleAsync_WorksCorrect()
        {
            var rolesCount = roles.Count();

            await adminService.CreateRoleAsync(new CreateRoleViewModel()
            {
                RoleName = "test"
            });

            var afterCreateCount = roles.Count();

            Assert.That(afterCreateCount, Is.EqualTo(rolesCount + 1));
        }
    }
}
