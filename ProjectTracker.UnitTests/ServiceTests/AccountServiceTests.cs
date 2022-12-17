using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.Services;
using ProjectTracker.Core.ViewModels.Account;
using ProjectTracker.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.UnitTests.ServiceTests
{
    public class AccountServiceTests : UnitTestsBase
    {
        private Mock<UserManager<Employee>> userManagerMock;
        private Mock<SignInManager<Employee>> signInManagerMock;
        private Mock<RoleManager<IdentityRole>> roleManagerMock;
        private Mock<IEmployeeService> employeeServiceMock;

        private IAccountService accountService;

        [SetUp]
        public async Task Setup()
        {
            var userStoreMock = new Mock<IUserStore<Employee>>();
            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            var contextAccesorMock = new Mock<IHttpContextAccessor>();
            var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<Employee>>();

            userManagerMock = new Mock<UserManager<Employee>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
            signInManagerMock = new Mock<SignInManager<Employee>>(
                userManagerMock.Object, 
                contextAccesorMock.Object, 
                claimsFactoryMock.Object, null, null, null, null);
            roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                roleStoreMock.Object, null, null, null, null);
            employeeServiceMock = new Mock<IEmployeeService>();

            var roles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            };

            //var manager = new SignInManager<Employee>()

            //Role manager setup
            roleManagerMock.Setup(rm => rm.Roles)
                .Returns(roles.AsQueryable());

            //User manager setup
            userManagerMock.Setup(um => um.CreateAsync(It.IsAny<Employee>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success))
                .Callback(async (Employee employee, string role) => data.Employees.AddAsync(employee));

            userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<Employee>(), It.IsAny<string>()).Result)
                .Returns(IdentityResult.Success);

            //Sign In Manager setup

            accountService = new AccountService(
                userManagerMock.Object,
                signInManagerMock.Object,
                roleManagerMock.Object,
                employeeServiceMock.Object);


        }

        [Test]
        public async Task GuestRegister_WorksCorrect()
        {
            var result = await accountService.GuestRegisterAsync(new GuestRegisterViewModel()
            {
                Role = "Admin"
            });

            Assert.That(result, Is.EqualTo(IdentityResult.Success));
        }

        [Test]
        public async Task GuestRegisterAsync_FailsIfRoleDoesntExist()
        {
            var result = await accountService.GuestRegisterAsync(new GuestRegisterViewModel()
            {
                Role = "blabla"
            });

            Assert.That(result.Succeeded, Is.EqualTo(false));
        }
    }
}
