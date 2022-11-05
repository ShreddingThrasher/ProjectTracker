using Microsoft.EntityFrameworkCore;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.ViewModels.Ticket;
using ProjectTracker.Core.ViewModels.Project;
using ProjectTracker.Infrastructure.Data.Common;
using ProjectTracker.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTracker.Infrastructure.Data.Entities.Enums;
using ProjectTracker.Core.ViewModels.Ticket.TicketComment;
using ProjectTracker.Core.ViewModels.Ticket.TicketChange;

namespace ProjectTracker.Core.Services
{
    public class TicketService : ITicketService
    {
        private readonly IRepository repo;

        public TicketService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task CreateTicketAsync(SubmitTicketViewModel model, string submitterId, Guid departmentId)
        {
            var ticket = new Ticket()
            {
                Title = model.Title,
                Description = model.Description,
                DepartmentId = departmentId,
                SubmitterId = submitterId,
                CreatedOn = DateTime.Now,
                ProjectId = model.ProjectId,
                Status = Status.Open,
                Priority = model.Priority
            };

            await repo.AddAsync(ticket);
            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<TicketViewModel>> GetAll()
        {
            return await repo.AllReadonly<Ticket>()
                .Where(t => t.IsActive)
                .Include(t => t.Project)
                .Select(t => new TicketViewModel()
                {
                    Id = t.Id,
                    Subbmitter = t.Submitter.UserName,
                    Title = t.Title,
                    Project = new ProjectIdNameViewModel()
                    {
                        Id = t.Project.Id,
                        Name = t.Project.Name
                    },
                    Priority = t.Priority,
                    Status = t.Status,
                    Date = t.CreatedOn
                })
                .ToListAsync();
        }

        public async Task<int> GetCount()
            => await this.repo.AllReadonly<Ticket>().Where(t => t.IsActive).CountAsync();

        public async Task<TicketDetailsViewModel> GetTicketDetaisById(Guid id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await repo.AllReadonly<Ticket>()
                .Where(t => t.IsActive && t.Id == id)
                .Include(t => t.Submitter)
                .Include(t => t.Project)
                .Include(t => t.AssignedEmployee)
                .Include(t => t.Comments)
                .ThenInclude(c => c.Commenter)
                .Include(t => t.History)
                .Select(t => new TicketDetailsViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    SubmitterId = t.SubmitterId,
                    Submitter = t.Submitter.UserName,
                    CreatedOn = t.CreatedOn,
                    ProjectId = t.ProjectId,
                    Project = t.Project.Name,
                    AssignedEmployeeId = t.AssignedEmployeeId,
                    AssignedEmployee = t.AssignedEmployee.UserName,
                    Status = t.Status.ToString(),
                    Priority = t.Priority.ToString(),
                    Comments = t.Comments
                        .Where(c => c.IsActive)
                        .Select(c => new TicketCommentViewModel()
                        {
                            Message = c.Message,
                            CreatedOn = c.CreatedOn,
                            CommenterId = c.CommenterId,
                            Commenter = c.Commenter.UserName
                        }).ToList(),
                    History = t.History
                        .Where(c => c.IsActive)
                        .Select(c => new TicketChangeViewModel()
                        {
                            Property = c.Property,
                            OldValue = c.OldValue,
                            NewValue = c.NewValue
                        }).ToList()
                })
                .FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
