using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProjectTracker.Infrastructure.Data;
using ProjectTracker.Infrastructure.Data.Entities;
using ProjectTracker.Infrastructure.Data.Entities.Enums;
using ProjectTracker.Infrastructure.DataConstants;
using ProjectTracker.UnitTests.Mocks;

namespace ProjectTracker.UnitTests
{
    public class UnitTestsBase
    {
        protected ProjectTrackerDbContext data;


        [OneTimeSetUp]
        public void SetUpBase()
        {
            data = DatabaseMock.instance;
            SeedDatabase();
        }

        [OneTimeTearDown]
        public void TearDownBase()
            => data.Dispose();

        private void SeedDatabase()
        {
            //departments
            data.Departments.Add(new Department()
            {
                Id = new Guid("94874ad5-4e3a-4bea-bac8-c7a3002eb205"),
                Name = "Development",
                LeadId = AdminConstants.AdminId,
                IsActive = true
            });

            //employees
            var employees = new List<Employee>()
            {
                new Employee()
                {
                    Id = "6da2e2f3-c43b-4a68-beea-4da7915b1528",
                    UserName = "DepartmentLeader",
                    FirstName = "Some test",
                    LastName = "AnotherTest",
                    Email = "tester@abv.bg",
                    LeadedDepartment = new Department()
                    {
                        Id = new Guid("aabcdf93-5aa9-46dc-bbf3-854d551a8b6d"),
                        Name = "Testing department",
                        LeadId = "6da2e2f3-c43b-4a68-beea-4da7915b1528",
                        IsActive = true
                    },
                    LeadedDepartmentId = new Guid("aabcdf93-5aa9-46dc-bbf3-854d551a8b6d"),
                    IsActive = true
                },
                new Employee()
                {
                    Id = "cd77e188-a292-42ed-b165-015b9f1d8c51",
                    UserName = "Second",
                    FirstName = "Second test",
                    LastName = "AnotherTest2",
                    Email = "second@abv.bg",
                    IsActive = true
                },
                new Employee()
                {
                    Id = "421365c1-f8d2-4a4b-abc7-aaabaa82117d",
                    UserName = "GuestTest",
                    FirstName = "Guest test",
                    LastName = "Guest testerrr",
                    Email = "guestTesteRR@mail.bg",
                    IsActive = true,
                    IsGuest = true
                }
            };

            data.Employees.AddRange(employees);

            //projects
            var projects = new List<Project>()
            {
                new Project()
                {
                    Id = new Guid("5ab2d1c9-13b1-48a2-b31d-546537e148c8"),
                    Name = "TestProject",
                    Description = "Done just for unit testing",
                    DepartmentId = new Guid("94874ad5-4e3a-4bea-bac8-c7a3002eb205"),
                    IsActive = true
                },
                new Project()
                {
                    Id = new Guid("bae3e81b-bfe0-418a-8082-4672bf1f98cd"),
                    Name = "SecondTestProject",
                    Description = "Another project for unit testing",
                    DepartmentId = new Guid("94874ad5-4e3a-4bea-bac8-c7a3002eb205"),
                    IsActive = true
                }
            };

            data.Projects.AddRange(projects);

            //EmployeeProjects
            var employeeProjects = new List<EmployeeProject>()
            {
                new EmployeeProject()
                {
                    EmployeeId = "6da2e2f3-c43b-4a68-beea-4da7915b1528",
                    ProjectId = new Guid("94874ad5-4e3a-4bea-bac8-c7a3002eb205"),
                    IsActive = true
                },
                new EmployeeProject()
                {
                    EmployeeId = "cd77e188-a292-42ed-b165-015b9f1d8c51",
                    ProjectId = new Guid("94874ad5-4e3a-4bea-bac8-c7a3002eb205"),
                    IsActive = true
                },
                new EmployeeProject()
                {
                    EmployeeId = "6da2e2f3-c43b-4a68-beea-4da7915b1528",
                    ProjectId = new Guid("bae3e81b-bfe0-418a-8082-4672bf1f98cd"),
                    IsActive = true
                },
                new EmployeeProject()
                {
                    EmployeeId = "cd77e188-a292-42ed-b165-015b9f1d8c51",
                    ProjectId = new Guid("bae3e81b-bfe0-418a-8082-4672bf1f98cd"),
                    IsActive = true
                },
                new EmployeeProject()
                {
                    EmployeeId = "421365c1-f8d2-4a4b-abc7-aaabaa82117d",
                    ProjectId = new Guid("bae3e81b-bfe0-418a-8082-4672bf1f98cd"),
                    IsActive = true
                }
            };

            data.EmployeesProjects.AddRange(employeeProjects);

            //Tickets
            var tickets = new List<Ticket>()
            {
                new Ticket()
                {
                    Id = new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"),
                    Title = "Ticket",
                    Description = "SomeDescription",
                    DepartmentId = new Guid("94874ad5-4e3a-4bea-bac8-c7a3002eb205"),
                    SubmitterId = "cd77e188-a292-42ed-b165-015b9f1d8c51",
                    CreatedOn = DateTime.Now,
                    ProjectId = new Guid("bae3e81b-bfe0-418a-8082-4672bf1f98cd"),
                    AssignedEmployeeId = null,
                    Status = Status.Open,
                    Priority = Priority.Low,
                    IsActive = true
                },
                new Ticket()
                {
                    Id = new Guid("eb9dd1d3-db59-4776-b9b8-fdb4287b18d0"),
                    Title = "Ticket 2",
                    Description = "SomeDescription lalalala",
                    DepartmentId = new Guid("94874ad5-4e3a-4bea-bac8-c7a3002eb205"),
                    SubmitterId = "6da2e2f3-c43b-4a68-beea-4da7915b1528",
                    CreatedOn = DateTime.Now,
                    ProjectId = new Guid("bae3e81b-bfe0-418a-8082-4672bf1f98cd"),
                    AssignedEmployeeId = "cd77e188-a292-42ed-b165-015b9f1d8c51",
                    Status = Status.Done,
                    Priority = Priority.High,
                    IsActive = true
                },
                new Ticket()
                {
                    Id = new Guid("5b2a9cbb-9b2a-41aa-8caf-1141d5b7bad1"),
                    Title = "Ticket - 3",
                    Description = "third ticket description",
                    DepartmentId = new Guid("94874ad5-4e3a-4bea-bac8-c7a3002eb205"),
                    SubmitterId = "cd77e188-a292-42ed-b165-015b9f1d8c51",
                    CreatedOn = DateTime.Now,
                    ProjectId = new Guid("94874ad5-4e3a-4bea-bac8-c7a3002eb205"),
                    AssignedEmployeeId = "6da2e2f3-c43b-4a68-beea-4da7915b1528",
                    Status = Status.InProgress,
                    Priority = Priority.Medium,
                    IsActive = true
                },
                new Ticket()
                {
                    Id = new Guid("6620234f-e55f-4b3f-bb06-5f74f8402c02"),
                    Title = "Ticket - 4",
                    Description = "yet another ticket description",
                    DepartmentId = new Guid("94874ad5-4e3a-4bea-bac8-c7a3002eb205"),
                    SubmitterId = "421365c1-f8d2-4a4b-abc7-aaabaa82117d",
                    CreatedOn = DateTime.Now,
                    ProjectId = new Guid("bae3e81b-bfe0-418a-8082-4672bf1f98cd"),
                    AssignedEmployeeId = "6da2e2f3-c43b-4a68-beea-4da7915b1528",
                    Status = Status.InProgress,
                    Priority = Priority.High,
                    IsActive = true
                }
            };

            data.Tickets.AddRange(tickets);

            //TicketComments
            var ticketComments = new List<TicketComment>()
            {
                new TicketComment()
                {
                    Id = new Guid("91b83512-48ef-46bb-85d3-0e299af6adf1"),
                    Message = "Test comment",
                    CreatedOn = DateTime.Now,
                    CommenterId = "6da2e2f3-c43b-4a68-beea-4da7915b1528",
                    TicketId = new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"),
                    IsActive = true
                },
                new TicketComment()
                {
                    Id = new Guid("b0b00c54-c76e-4ca0-8ce2-d475793bcf52"),
                    Message = "Test comment - second",
                    CreatedOn = DateTime.Now,
                    CommenterId = "6da2e2f3-c43b-4a68-beea-4da7915b1528",
                    TicketId = new Guid("5b2a9cbb-9b2a-41aa-8caf-1141d5b7bad1"),
                    IsActive = true
                },
                new TicketComment()
                {
                    Id = new Guid("3167b484-6107-4c8f-bf85-c9c1c32cb391"),
                    Message = "Test comment - third",
                    CreatedOn = DateTime.Now,
                    CommenterId = "421365c1-f8d2-4a4b-abc7-aaabaa82117d",
                    TicketId = new Guid("5b2a9cbb-9b2a-41aa-8caf-1141d5b7bad1"),
                    IsActive = true
                }
            };

            data.Comments.AddRange(ticketComments);

            //TicketChange
            var ticketChanges = new List<TicketChange>()
            {
                new TicketChange()
                {
                    Id = new Guid("60a289f3-3fe6-4c75-a2da-635e0d189720"),
                    Property = nameof(Ticket.Status),
                    OldValue = Status.Open.ToString(),
                    NewValue = Status.Done.ToString(),
                    Date = DateTime.Now,
                    TicketId = new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"),
                    IsActive = true
                },
                new TicketChange()
                {
                    Id = new Guid("862d2155-01cf-414a-b3cb-b84671ed9179"),
                    Property = nameof(Ticket.AssignedEmployeeId),
                    OldValue = string.Empty,
                    NewValue = "cd77e188-a292-42ed-b165-015b9f1d8c51",
                    Date = DateTime.Now,
                    TicketId = new Guid("85677f6d-687b-4245-ab81-3c198cfaa831"),
                    IsActive = true
                }
            };

            data.SaveChanges();

            data.Changes.AddRange(ticketChanges);
        }
    }
}

