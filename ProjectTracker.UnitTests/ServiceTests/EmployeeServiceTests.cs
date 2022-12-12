using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.Services;
using ProjectTracker.Infrastructure.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.UnitTests.ServiceTests
{
    [TestFixture]
    public class EmployeeServiceTests : UnitTestsBase
    {
        private IRepository repo;
        private IEmployeeService employeeService;

        [SetUp]
        public async Task Setup()
        {
            SetUpBase();

            repo = new Repository(data);
            employeeService = new EmployeeService(repo);

            await SetAllToActive();
        }

        [Test]
        public async Task GetAllAsync_ReturnsCorrect()
        {
            var expected = await data.Employees.Where(e => e.IsActive).CountAsync();

            var actual = employeeService.GetAllAsync().Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetCountAsync_ReturnsCorrect()
        {
            var expected = await data.Employees.Where(e => e.IsActive).CountAsync();

            var actual = await employeeService.GetCountAsync();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetAllIdAndNameAsync_ReturnsCorrect()
        {
            var expected = await data.Employees.Where(e => e.IsActive).CountAsync();

            var actual = employeeService.GetAllIdAndNameAsync().Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetUserNamesAsync_ReturnsCorrect()
        {
            var expected = await data.Employees.Where(e => e.IsActive).CountAsync();

            var actual = employeeService.GetUserNamesAsync().Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetEmployeeDetailsAsync_ReturnsCorrect()
        {
            string employeeId = "421365c1-f8d2-4a4b-abc7-aaabaa82117d";
            string expectedUserName = "GuestTest";
            string expectedFirstName = "Guest test";
            string expectedLastName = "Guest testerrr";
            string expectedEmail = "guestTesteRR@mail.bg";

            var employee = await employeeService.GetEmployeeDetailsAsync(employeeId);

            Assert.That(employee.UserName, Is.EqualTo(expectedUserName));
            Assert.That(employee.FirstName, Is.EqualTo(expectedFirstName));
            Assert.That(employee.LastName, Is.EqualTo(expectedLastName));
            Assert.That(employee.Email, Is.EqualTo(expectedEmail));
        }

        [Test]
        public async Task GetUnassignedAsync_ReturnsCorrect()
        {
            var expected = await data.Employees.Where(e => e.IsActive && e.DepartmentId == null).CountAsync();

            var actual = employeeService.GetUnassignedAsync().Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetActiveAsync_ReturnsCorrect()
        {
            var expected = await data.Employees.Where(e => e.IsActive && e.DepartmentId != null).CountAsync();

            var actual = employeeService.GetActiveAsync().Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task RemoveById_RemovesCorrect()
        {
            var startStatus = data.Employees
                .Where(e => e.Id == "421365c1-f8d2-4a4b-abc7-aaabaa82117d")
                .First().IsActive;

            await employeeService.RemoveById("421365c1-f8d2-4a4b-abc7-aaabaa82117d");

            var actual = data.Employees
                .Where(e => e.Id == "421365c1-f8d2-4a4b-abc7-aaabaa82117d")
                .First().IsActive;

            Assert.That(startStatus);
            Assert.That(!actual);
            Assert.That(actual != startStatus);
        }

        [Test]
        public void RemoveById_ThrowsNullReferenceException_IfEmployeeDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(async ()
                => await employeeService.RemoveById("not existent"));
        }

        [Test]
        public async Task RemoveById_SetsEmployeesProjectsToInactive()
        {
            await employeeService.RemoveById("421365c1-f8d2-4a4b-abc7-aaabaa82117d");

            var employee = await data.Employees
                .Where(e => e.Id == "421365c1-f8d2-4a4b-abc7-aaabaa82117d")
                .Include(e => e.EmployeesProjects)
                .FirstAsync();

            foreach (var project in employee.EmployeesProjects)
            {
                Assert.That(!project.IsActive);
            }
        }

        [Test]
        public async Task RemoveById_SetsEmployeesTicketsToInactive()
        {
            await employeeService.RemoveById("421365c1-f8d2-4a4b-abc7-aaabaa82117d");

            var employee = await data.Employees
                .Where(e => e.Id == "421365c1-f8d2-4a4b-abc7-aaabaa82117d")
                .Include(e => e.SubmittedTickets)
                .FirstAsync();

            foreach (var ticket in employee.SubmittedTickets)
            {
                Assert.That(!ticket.IsActive);
            }
        }

        [Test]
        public async Task RemoveById_SetsAssignedTicketsToUnassigned()
        {
            await employeeService.RemoveById("421365c1-f8d2-4a4b-abc7-aaabaa82117d");

            var employee = await data.Employees
                .Where(e => e.Id == "421365c1-f8d2-4a4b-abc7-aaabaa82117d")
                .Include(e => e.AssignedTickets)
                .FirstAsync();

            foreach (var ticket in employee.AssignedTickets)
            {
                Assert.That(ticket.AssignedEmployeeId == null);
            }
        }

        private async Task SetAllToActive()
        {
            await data.Employees.ForEachAsync(
                p => p.IsActive = true);

            await data.SaveChangesAsync();
        }
    }
}