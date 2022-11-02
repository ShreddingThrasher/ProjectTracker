using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Department
{
	public class DepartmentViewModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Employees { get; set; }
		public int Projects { get; set; }
		public int Tickets { get; set; }

		public string LeaderId { get; set; }
	}
}
