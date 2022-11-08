using ProjectTracker.Core.Constants;
using ProjectTracker.Core.Contracts;
using ProjectTracker.Core.Services;
using ProjectTracker.Infrastructure.Data.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ProjectTrackerServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IAdminService, AdminService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanAssignAndEditTicket", policy =>
                {
                    policy.RequireRole(new string[] { RoleConstants.Admin, RoleConstants.DepartmentLead });
                });
            });

            return services;
        }
    }
}
