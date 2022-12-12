using Microsoft.EntityFrameworkCore;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.Services;
using ProjectTracker.Core.ViewModels.Ticket;
using ProjectTracker.Core.ViewModels.Ticket.TicketComment;
using ProjectTracker.Infrastructure.Data.Common;
using ProjectTracker.Infrastructure.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.UnitTests.ServiceTests
{
    [TestFixture]
    public class TicketServiceTests : UnitTestsBase
    {
        private IRepository repo;
        private ITicketService ticketService;

        [SetUp]
        public async Task Setup()
        {
            repo = new Repository(data);
            ticketService = new TicketService(repo);

            await SetAllToActive();
        }

        [Test]
        public async Task CreateCommentAsync_CreatesCorrect()
        {
            var ticket = await data.Tickets
                .Where(t => t.IsActive && t.Id == new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"))
                .Include(t => t.Comments)
                .FirstOrDefaultAsync();

            var startCount = ticket.Comments.Count();

            await ticketService.CreateCommentAsync("6da2e2f3-c43b-4a68-beea-4da7915b1528", new CreateTicketCommentViewModel()
            {
                TicketId = new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"),
                Message = "this is a comment"
            });

            var countAfterCreate = ticket.Comments.Count();

            Assert.That(countAfterCreate, Is.EqualTo(startCount + 1));
        }

        [Test]
        public async Task CreateCommentAsync_ThrowsNullReferenceException_WhenTicketDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(async ()
                => await ticketService.CreateCommentAsync("6da2e2f3-c43b-4a68-beea-4da7915b1528", new CreateTicketCommentViewModel()
                {
                    TicketId = Guid.NewGuid(),
                    Message = "this is a comment"
                }));
        }

        [Test]
        public async Task CreateTicketAsync_CreatesCorrect()
        {
            var startCount = await data.Tickets
                .Where(t => t.IsActive)
                .CountAsync();

            var model = new SubmitTicketViewModel()
            {
                ProjectId = new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8"),
                Title = "test test",
                Description = "test test",
                Priority = Infrastructure.Data.Entities.Enums.Priority.Low
            };

            await ticketService.CreateTicketAsync(
                model, "6da2e2f3-c43b-4a68-beea-4da7915b1528", new Guid("94874ad5-4e3a-4bea-bac8-c7a3002eb205"));

            var countAfterCreate = await data.Tickets
                .Where(t => t.IsActive)
                .CountAsync();

            Assert.That(countAfterCreate, Is.EqualTo(startCount + 1));
        }

        [Test]
        public async Task EditTicketAsync_EditsTicketCorrect()
        {
            var ticket = await data.Tickets
                .Where(t => t.IsActive && t.Id == new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"))
                .FirstOrDefaultAsync();

            var oldTitle = ticket.Title;
            var oldPriority = ticket.Priority;
            var oldStatus = ticket.Status;

            var newTitle = "test edit";
            var newPriority = Priority.Medium;
            var newStatus = Status.Done;

            await ticketService.EditTicketAsync(new EditTicketViewModel()
            {
                Id = new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"),
                Title = newTitle,
                Priority = newPriority,
                Status = newStatus
            });

            Assert.That(oldTitle, Is.Not.EqualTo(ticket.Title));
            Assert.That(oldPriority, Is.Not.EqualTo(ticket.Priority));
            Assert.That(oldStatus, Is.Not.EqualTo(ticket.Status));

            Assert.That(ticket.Title, Is.EqualTo(newTitle));
            Assert.That(ticket.Priority, Is.EqualTo(newPriority));
            Assert.That(ticket.Status, Is.EqualTo(newStatus));
        }

        [Test]
        public async Task EditTicketAsync_AddsTicketChanges()
        {
            var ticket = await data.Tickets
                .Where(t => t.IsActive && t.Id == new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"))
                .Include(t => t.History)
                .FirstOrDefaultAsync();

            var oldTitle = ticket.Title;
            var oldPriority = ticket.Priority;
            var oldStatus = ticket.Status;

            var newTitle = "editeeeed";
            var newPriority = Priority.High;
            var newStatus = Status.InProgress;

            var startCount = ticket.History.Count();

            await ticketService.EditTicketAsync(new EditTicketViewModel()
            {
                Id = new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"),
                Title = newTitle,
                Priority = newPriority,
                Status = newStatus
            });

            var countAfterEdit = ticket.History.Count();

            Assert.That(countAfterEdit, Is.EqualTo(startCount + 3));
        }

        [Test]
        public async Task GetAllAsync_ReturnsCorrect()
        {
            var expected = await data.Tickets
                .Where(t => t.IsActive)
                .CountAsync();

            var tickets = await data.Tickets
                .Where(t => t.IsActive)
                .ToListAsync();

            var service = await ticketService.GetAllAsync();

            var actual = ticketService.GetAllAsync().Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCorrect()
        {
            var expected = await data.Tickets
                .Where(t => t.IsActive && t.Id == new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"))
                .FirstOrDefaultAsync();

            var actual = await ticketService.GetByIdAsync(new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"));

            Assert.That(expected.Id == actual.Id);
            Assert.That(expected.Title == actual.Title);
            Assert.That(expected.Priority == actual.Priority);
            Assert.That(expected.Status == actual.Status);
        }

        [Test]
        public async Task GetCountAsync_ReturnsCorrect()
        {
            var expected = await data.Tickets
                .Where(t => t.IsActive)
                .CountAsync();

            var actual = await ticketService.GetCountAsync();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetTicketDetailsByIdAsync_ReturnsCorrect()
        {
            var expected = await data.Tickets
                .Where(t => t.IsActive && t.Id == new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"))
                .FirstOrDefaultAsync();

            var actual = await ticketService.GetTicketDetailsByIdAsync(new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"));

            Assert.That(expected.Id == actual.Id);
            Assert.That(expected.Title == actual.Title);
            Assert.That(expected.Priority.ToString() == actual.Priority);
            Assert.That(expected.Status.ToString() == actual.Status);
        }

        [Test]
        public async Task AssignTicketAsync_ThrowsNullReferenceException_WhenEmployeeDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(async ()
                => await ticketService.AssignTicketAsync(new AssignTicketViewModel()
                {
                    TicketId = new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"),
                    EmployeeId = "non existent"
                }));
        }

        [Test]
        public async Task AssignTicketAsync_ThrowsNullReferenceException_WhenTicketDoesntExist()
        {
            Assert.ThrowsAsync<NullReferenceException>(async ()
                => await ticketService.AssignTicketAsync(new AssignTicketViewModel()
                {
                    TicketId = Guid.NewGuid(),
                    EmployeeId = "6da2e2f3-c43b-4a68-beea-4da7915b1528"
                }));
        }

        [Test]
        public async Task AssignTicketAsync_ThrowsArgumentException_WhenItsAlreadyAssignedToThisEmployee()
        {
            Assert.ThrowsAsync<ArgumentException>(async ()
                => await ticketService.AssignTicketAsync(new AssignTicketViewModel()
                {
                    TicketId = new Guid("eb9dd1d3-db59-4776-b9b8-fdb4287b18d0"),
                    EmployeeId = "cd77e188-a292-42ed-b165-015b9f1d8c51"
                }));
        }

        [Test]
        public async Task AssignTicketAsync_ThrowsArgumentException_WhenTicketIsAlreadyDone()
        {
            Assert.ThrowsAsync<ArgumentException>(async ()
                => await ticketService.AssignTicketAsync(new AssignTicketViewModel()
                {
                    TicketId = new Guid("eb9dd1d3-db59-4776-b9b8-fdb4287b18d0"),
                    EmployeeId = "6da2e2f3-c43b-4a68-beea-4da7915b1528"
                }));
        }

        [Test]
        public async Task AssignTicketAsync_CreatesChange_And_SetsStatusToInProgress_IfItsOpen()
        {
            var ticket = await data.Tickets
                .Where(t => t.IsActive && t.Id == new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"))
                .Include(t => t.History)
                .FirstOrDefaultAsync();

            var startHistory = ticket.History.Count();
            var startStatus = ticket.Status;

            await ticketService.AssignTicketAsync(new AssignTicketViewModel()
            {
                TicketId = new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"),
                EmployeeId = "6da2e2f3-c43b-4a68-beea-4da7915b1528"
            });

            var historyAfterAssign = ticket.History.Count();
            var statusAfterAssign = ticket.Status;

            Assert.That(historyAfterAssign, Is.EqualTo(startHistory + 2));
            Assert.That(startStatus, Is.Not.EqualTo(statusAfterAssign));
            Assert.That(startStatus, Is.EqualTo(Status.Open));
            Assert.That(statusAfterAssign, Is.EqualTo(Status.InProgress));

            //revert data so it doesnt affect other tests
            ticket.History.Clear();
            ticket.AssignedEmployee = null;
            ticket.Status = Status.Open;
        }

        [Test]
        public async Task AssignTicketAsync_AssignsToEmployee_And_CreatesChange()
        {
            var ticket = await data.Tickets
                .Where(t => t.IsActive && t.Id == new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"))
                .Include(t => t.History)
                .FirstOrDefaultAsync();

            ticket.Status = Status.InProgress;
            var startHistory = ticket.History.Count();
            var startAssignedEmployeeId = ticket.AssignedEmployeeId;

            await ticketService.AssignTicketAsync(new AssignTicketViewModel()
            {
                TicketId = new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"),
                EmployeeId = "6da2e2f3-c43b-4a68-beea-4da7915b1528"
            });

            var historyAfterAssign = ticket.History.Count();
            var statusAfterAssign = ticket.Status;

            Assert.That(historyAfterAssign, Is.EqualTo(startHistory + 1));
            Assert.That(statusAfterAssign, Is.EqualTo(Status.InProgress));
            Assert.That(startAssignedEmployeeId, Is.Not.EqualTo(ticket.AssignedEmployeeId));
            Assert.That(ticket.AssignedEmployeeId, Is.EqualTo("6da2e2f3-c43b-4a68-beea-4da7915b1528"));

            //revert data so it doesnt affect other tests
            ticket.History.Clear();
            ticket.AssignedEmployee = null;
            ticket.Status = Status.Open;
        }

        [Test]
        public async Task GetAllStatusesAsync_ReturnsCorrect()
        {
            var expected = await data.Tickets
                .Where(t => t.IsActive)
                .Select(t => t.Status)
                .CountAsync();

            var actual = ticketService.GetAllStatusesAsync().Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetInProgressAsync_ReturnsCorrect()
        {
            var expected = await data.Tickets
                .Where(t => t.IsActive && t.Status == Status.InProgress)
                .CountAsync();

            var actual = ticketService.GetInProgressAsync().Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetDoneAsync_ReturnsCorrect()
        {
            var expected = await data.Tickets
                .Where(t => t.IsActive && t.Status == Status.Done)
                .CountAsync();

            var actual = ticketService.GetDoneAsync().Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetUnassignedAsync_ReturnsCorrect()
        {
            var expected = await data.Tickets
                .Where(t => t.IsActive && t.Status == Status.Open)
                .CountAsync();

            var actual = ticketService.GetUnassignedAsync().Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetPastAsync_ReturnsCorrect()
        {
            var expected = await data.Tickets
                .Where(t => !t.IsActive)
                .CountAsync();

            var actual = ticketService.GetPastAsync().Result.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        private async Task SetAllToActive()
        {
            await data.Tickets.ForEachAsync(
                p => p.IsActive = true);

            await data.SaveChangesAsync();
        }
    }
}
