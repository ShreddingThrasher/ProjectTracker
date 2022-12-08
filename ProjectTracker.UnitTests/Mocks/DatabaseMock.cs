using Microsoft.EntityFrameworkCore;
using ProjectTracker.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.UnitTests.Mocks
{
    public class DatabaseMock
    {
        public static ProjectTrackerDbContext instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<ProjectTrackerDbContext>()
                    .UseInMemoryDatabase("ProjectTrackerInMemoryDb"
                        + DateTime.Now.Ticks.ToString())
                    .Options;

                return new ProjectTrackerDbContext(dbContextOptions);
            }
        }
    }
}
