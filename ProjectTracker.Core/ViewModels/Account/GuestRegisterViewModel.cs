using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.ViewModels.Account
{
	public class GuestRegisterViewModel
	{
		[Required]
		public string Role { get; set; }
		public IEnumerable<string> Roles { get; set; } = new List<string>();
	}
}
